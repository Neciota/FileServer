using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;

namespace FileServer.Server.Services.Interfaces
{
    public interface IFolderService
    {
        Task<ServiceResponse<FolderResponse>> CreateFolderAsync(CreateFolderRequest request, Guid accountGuid);
        Task<ServiceResponse<IEnumerable<TopFolderResponse>>> GetTopFoldersAsync(Guid accountGuid);
    }
}
