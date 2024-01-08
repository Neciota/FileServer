using FileServer.Models.Responses;
using FileServer.Server.Services.Interfaces;

namespace FileServer.Server.Services
{
    public class UserRequestService : IUserRequestService
    {
        public UserResponse? User { get; set; }

        public bool HasUser => User is not null;
    }
}
