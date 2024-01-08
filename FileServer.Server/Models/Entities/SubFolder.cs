namespace FileServer.Server.Models.Entities
{
    public class SubFolder : Folder
    {
        public int ParentId { get; set; }
        public Folder Parent { get; set; }
        public ICollection<Folder> Children { get; set; } = new List<Folder>();
    }
}
