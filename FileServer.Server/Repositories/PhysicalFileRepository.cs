using FileServer.Server.Models;
using FileServer.Server.Repositories.Interfaces;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace FileServer.Server.Repositories
{
    public class PhysicalFileRepository: IPhysicalFileRepository
    {
        private readonly IOptions<DiskSettings> _diskSettings;

        public PhysicalFileRepository(IOptions<DiskSettings> diskSettings)
        {
            _diskSettings = diskSettings;
        }

        public FileStream GetFile(string physicalPath)
        {
            return new FileStream(GetFullPath(physicalPath), FileMode.Open);
        }

        public async Task<string> AddFileAsync(Guid accountGuid, Guid fileGuid, IFormFile file)
        {
            string fullPath = GetFullPath(accountGuid, fileGuid);

            using StreamWriter writer = new StreamWriter(fullPath);
            using StreamReader reader = new StreamReader(file.OpenReadStream());
            while (!reader.EndOfStream)
            {
                char[] buffer = new char[1024];
                await reader.ReadBlockAsync(buffer);
                await writer.WriteAsync(buffer);
            }

            return fullPath;
        }

        private string GetFullPath(Guid accountGuid, Guid fileGuid) => Path.Combine(_diskSettings.Value.RootFolder, accountGuid.ToString(), fileGuid.ToString());

        private string GetFullPath(string subpath) => Path.Combine(_diskSettings.Value.RootFolder, subpath);
    }
}
