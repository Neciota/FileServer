using FileServer.Server.Data;
using FileServer.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using File = FileServer.Server.Models.Entities.File;

namespace FileServer.Server.Repositories
{
    public class FileRepository: IFileRepository
    {
        private readonly DataContext _context;

        public FileRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<File> AddFileAsync(File file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return file;
        }

        public async Task<File?> GetFileWithFolderAsync(Guid guid)
        {
            return await _context.Files.Include(file => file.Folder)
                .FirstOrDefaultAsync(x => x.Guid == guid);
        }
    }
}
