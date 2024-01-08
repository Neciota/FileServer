namespace FileServer.Server.Models.Entities
{
    public abstract class Folder
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
