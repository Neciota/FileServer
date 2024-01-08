using File = FileServer.Server.Models.Entities.File;

namespace FileServer.Server.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Task<File> AddFileAsync(File file);
        Task<File?> GetFileWithFolderAsync(Guid guid);
    }
}
