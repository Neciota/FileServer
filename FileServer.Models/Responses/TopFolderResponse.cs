namespace FileServer.Models.Responses
{
    public class TopFolderResponse: FolderResponse
    {
        public IEnumerable<UserResponse> Owners { get; set; } = Enumerable.Empty<UserResponse>();
    }
}
