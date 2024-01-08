using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;

namespace FileServer.Server.Services.Interfaces
{
    public interface IFileService
    {
        Task<ServiceResponse<(FileStream, string)>> DownloadFileAsync(DownloadFileRequest request, Guid downloaderGuid);
        Task<ServiceResponse<FileResponse>> UploadFileAsync(UploadFileRequest request, IFormFile file, Guid uploaderGuid);
    }
}
