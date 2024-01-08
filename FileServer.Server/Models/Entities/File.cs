namespace FileServer.Server.Models.Entities
{
    public class File
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public Folder Folder { get; set; }
        public string PhysicalPath { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
