using FileServer.Models.Responses;

namespace FileServer.Server.Services.Interfaces
{
    public interface IUserRequestService
    {
        UserResponse? User { get; set; }
        bool HasUser { get; }
    }
}
