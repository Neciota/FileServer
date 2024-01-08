using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using FileServer.Server.Services.Interfaces;
using File = FileServer.Server.Models.Entities.File;

namespace FileServer.Server.Services
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IPhysicalFileRepository _physicalFileRepository;
        private readonly IFolderRepository _folderRepository;

        public FileService(IFileRepository fileRepository, IFolderRepository folderRepository, IPhysicalFileRepository physicalFileRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
            _physicalFileRepository = physicalFileRepository;
        }

        public async Task<ServiceResponse<FileResponse>> UploadFileAsync(UploadFileRequest request, IFormFile file, Guid uploaderGuid)
        {
            Folder? parentFolder = await _folderRepository.GetFolderAsync(request.FolderGuid);
            if (parentFolder is null)
                return ServiceResponse<FileResponse>.Reject("No folder by the given guid.");

            TopFolder topFolder = await _folderRepository.GetTopFolderFromSubFolderAsync(request.FolderGuid);
            if (topFolder.Owners.All(owner => owner.Guid != uploaderGuid))
                return ServiceResponse<FileResponse>.Reject("Folder does not belong to uploader.");

            Guid fileGuid = Guid.NewGuid();
            string physicalPath = await _physicalFileRepository.AddFileAsync(uploaderGuid, fileGuid, file);

            File fileToAdd = new File()
            {
                Guid = fileGuid,
                Name = request.Name,
                Folder = parentFolder,
                PhysicalPath = physicalPath,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };
            File addedFile = await _fileRepository.AddFileAsync(fileToAdd);

            return ServiceResponse<FileResponse>.Accept(new FileResponse()
            {
                Guid = addedFile.Guid,
                Name = addedFile.Name
            });
        }

        public async Task<ServiceResponse<(FileStream, string)>> DownloadFileAsync(DownloadFileRequest request, Guid downloaderGuid)
        {
            File? file = await _fileRepository.GetFileWithFolderAsync(request.Guid);
            if (file is null)
                return ServiceResponse<(FileStream, string)>.Reject("No file with this guid present.");

            IEnumerable<Account> owners = await _folderRepository.GetFolderOwnersAsync(file.Folder.Guid);
            if (owners.All(owner => owner.Guid != downloaderGuid))
                return ServiceResponse<(FileStream, string)>.Reject("No access to the file.");

            return ServiceResponse<(FileStream, string)>.Accept((_physicalFileRepository.GetFile(file.PhysicalPath), file.Name));
        }
    }
}
