

namespace FileServer.Server.Repositories.Interfaces
{
    public interface IPhysicalFileRepository
    {
        Task<string> AddFileAsync(Guid accountGuid, Guid fileGuid, IFormFile file);
        FileStream GetFile(string physicalPath);
    }
}
