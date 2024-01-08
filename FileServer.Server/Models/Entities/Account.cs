using FileServer.Models;
using FileServer.Models.Responses;

namespace FileServer.Server.Models.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserType Type { get; set; } = UserType.User;
        public bool Approved { get; set; } = false;
        public int StorageLimit { get; set; }
        public ICollection<TopFolder> TopFolders { get; set; } = new List<TopFolder>();

        public UserResponse GetUserResponse() => new UserResponse()
        {
            Guid = Guid,
            UserName = UserName,
            Type = Type
        };
    }
}
