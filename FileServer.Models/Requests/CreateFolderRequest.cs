namespace FileServer.Models.Requests
{
    public class CreateFolderRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid? Parent { get; set; }
    }
}
