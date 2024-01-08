using Microsoft.AspNetCore.Http;

namespace FileServer.Models.Requests
{
    public class UploadFileRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid FolderGuid { get; set; }
        public IFormFile File { get; set; }
    }
}
