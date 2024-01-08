using FileServer.Server.Models.Entities;

namespace FileServer.Server.Repositories.Interfaces
{
    public interface IFolderRepository
    {
        Task<SubFolder> AddSubFolderAsync(SubFolder subFolder);
        Task<TopFolder> AddTopFolderAsync(TopFolder topFolder);
        Task<Folder?> GetFolderAsync(Guid guid);
        Task<IEnumerable<Account>> GetFolderOwnersAsync(Guid folderGuid);
        Task<IEnumerable<SubFolder>> GetSubFoldersByParentAsync(IEnumerable<int> parentIds);
        Task<TopFolder> GetTopFolderFromSubFolderAsync(Guid subfolderGuid);
        Task<IEnumerable<TopFolder>> GetTopLevelFoldersAsync(Guid accountGuid);
    }
}
