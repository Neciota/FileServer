namespace FileServer.Models.Responses
{
    public class FolderResponse
    {
        public Guid Guid { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public IList<SubFolderResponse> SubFolders { get; set; } = new List<SubFolderResponse>();
    }
}
