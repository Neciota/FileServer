namespace FileServer.Server.Models.Entities
{
    public class TopFolder : Folder
    {
        public ICollection<Account> Owners { get; set; } = new List<Account>();
        public ICollection<SubFolder> SubFolders { get; set; } = new List<SubFolder>();
    }
}
