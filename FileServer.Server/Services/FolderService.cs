using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using FileServer.Server.Services.Interfaces;
using System.Collections.Generic;

namespace FileServer.Server.Services
{
    public class FolderService : IFolderService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IFolderRepository _folderRepository;

        public FolderService(IAccountRepository accountRepository, IFolderRepository folderRepository)
        {
            _accountRepository = accountRepository;
            _folderRepository = folderRepository;
        }

        public async Task<ServiceResponse<IEnumerable<TopFolderResponse>>> GetTopFoldersAsync(Guid accountGuid)
        {
            IEnumerable<TopFolder> topFolders = await _folderRepository.GetTopLevelFoldersAsync(accountGuid);
            Dictionary<int, TopFolderResponse> topLayer = topFolders.ToDictionary(folder => folder.Id, folder => new TopFolderResponse()
            {
                Guid = folder.Guid,
                Name = folder.Name,
                Created = folder.Created,
                Updated = folder.Updated,
                SubFolders = new List<SubFolderResponse>()
            });
            Dictionary<int, FolderResponse> previousLayer = topLayer.ToDictionary(x => x.Key, x => x.Value as FolderResponse);

            IEnumerable<int> parentIds = topFolders.Select(folder => folder.Id);
            while (parentIds.Any())
            {
                IEnumerable<SubFolder> subFolders = await _folderRepository.GetSubFoldersByParentAsync(topFolders.Select(folder => folder.Id));
                parentIds = subFolders.Select(folder => folder.Id);

                Dictionary<int, FolderResponse> nextLayer = new Dictionary<int, FolderResponse>();
                foreach (SubFolder subFolder in subFolders)
                {
                    SubFolderResponse subFolderResponse = new SubFolderResponse()
                    {
                        Guid = subFolder.Guid,
                        Name = subFolder.Name,
                        Created = subFolder.Created,
                        Updated = subFolder.Updated,
                        SubFolders = new List<SubFolderResponse>()
                    };

                    nextLayer[subFolder.Id] = subFolderResponse;
                    previousLayer[subFolder.ParentId].SubFolders.Add(subFolderResponse);
                }

                previousLayer = nextLayer;
            }

            return ServiceResponse<IEnumerable<TopFolderResponse>>.Accept(topLayer.Values.ToList());
        }

        public async Task<ServiceResponse<FolderResponse>> CreateFolderAsync(CreateFolderRequest request, Guid accountGuid)
        {
            Account? owner = await _accountRepository.GetAccountByGuidAsync(accountGuid);

            if (owner is null)
                return ServiceResponse<FolderResponse>.Reject("No account with this Guid exists.");

            Folder addedFolder;
            if (request.Parent is null)
            {
                TopFolder newTopFolder = new TopFolder()
                {
                    Guid = Guid.NewGuid(),
                    Name = request.Name,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Owners = new[] { owner }
                };

                addedFolder = await _folderRepository.AddTopFolderAsync(newTopFolder);
            } else
            {
                Folder? parentFolder = await _folderRepository.GetFolderAsync(request.Parent.Value);

                if (parentFolder is null)
                    return ServiceResponse<FolderResponse>.Reject("No parent folder with this Guid exists.");

                SubFolder newSubFolder = new SubFolder()
                {
                    Guid = Guid.NewGuid(),
                    Name = request.Name,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    ParentId = parentFolder.Id
                };

                addedFolder = await _folderRepository.AddSubFolderAsync(newSubFolder);
            }

            return ServiceResponse<FolderResponse>.Accept(new FolderResponse()
            {
                Guid = addedFolder.Guid,
                Name = addedFolder.Name,
                Created = addedFolder.Created,
                Updated = addedFolder.Updated,
                SubFolders = Array.Empty<SubFolderResponse>()
            });
        }
    }
}
