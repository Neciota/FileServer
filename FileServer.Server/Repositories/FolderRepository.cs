using FileServer.Server.Data;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileServer.Server.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly DataContext _context;

        public FolderRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<Folder?> GetFolderAsync(Guid guid)
        {
            Folder? topFolder = await _context.TopFolders.FirstAsync(folder => folder.Guid == guid);
            
            if (topFolder is not null)
                return topFolder;

            return await _context.SubFolders.FirstAsync(folder => folder.Guid == guid);
        }

        public async Task<IEnumerable<TopFolder>> GetTopLevelFoldersAsync(Guid accountGuid)
        {
            return await _context.TopFolders.Where(folder => folder.Owners.Any(owner => owner.Guid == accountGuid))
                .ToListAsync();
        }

        public async Task<TopFolder> GetTopFolderFromSubFolderAsync(Guid subfolderGuid)
        {
            TopFolder? rootFolder = await _context.TopFolders.FirstOrDefaultAsync(folder => folder.Guid == subfolderGuid);
            if (rootFolder is not null)
                return rootFolder;

            SubFolder folder = await _context.SubFolders.Include(folder => folder.Parent)
                .FirstAsync(folder => folder.Guid == subfolderGuid);

            while (folder.Parent is SubFolder)
            {
                folder = await _context.SubFolders.Include(folder => folder.Parent)
                    .FirstAsync(folder => folder.Guid == subfolderGuid);
            }

            return (TopFolder)folder.Parent;
        }

        public async Task<IEnumerable<SubFolder>> GetSubFoldersByParentAsync(IEnumerable<int> parentIds)
        {
            return await _context.SubFolders.Where(folder => parentIds.Contains(folder.Parent.Id))
                .ToListAsync();
        }

        public async Task<TopFolder> AddTopFolderAsync(TopFolder topFolder)
        {
            _context.TopFolders.Add(topFolder);
            await _context.SaveChangesAsync();

            return topFolder;
        }

        public async Task<SubFolder> AddSubFolderAsync(SubFolder subFolder)
        {
            _context.SubFolders.Add(subFolder);
            await _context.SaveChangesAsync();

            return subFolder;
        }

        public async Task<IEnumerable<Account>> GetFolderOwnersAsync(Guid folderGuid)
        {
            TopFolder topFolder = await GetTopFolderFromSubFolderAsync(folderGuid);
            return await _context.TopFolders.Where(folder => folder.Id == topFolder.Id)
                .SelectMany(folder => folder.Owners)
                .ToListAsync();
        }
    }
}
