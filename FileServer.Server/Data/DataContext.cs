using FileServer.Server.Models;
using FileServer.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using File = FileServer.Server.Models.Entities.File;

namespace FileServer.Server.Data
{
    public class DataContext : DbContext
    {
        private readonly IOptions<ConnectionSettings> _connectionSettings;

        public DataContext(IOptions<ConnectionSettings> connectionSettings)
        {
            _connectionSettings = connectionSettings;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<SubFolder> SubFolders { get; set; }
        public DbSet<TopFolder> TopFolders { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionSettings.Value.SqliteDb);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>();
            modelBuilder.Entity<File>();
            modelBuilder.Entity<SubFolder>();
            modelBuilder.Entity<TopFolder>();
        }
    }
}
