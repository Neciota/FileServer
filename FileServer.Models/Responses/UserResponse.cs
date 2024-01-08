namespace FileServer.Models.Responses
{
    public class UserResponse
    {
        public Guid Guid { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserType Type { get; set; } = UserType.User;
    }
}
