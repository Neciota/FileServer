using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;

namespace FileServer.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ServiceResponse<string>> LoginAsync(LoginRequest request);
        Task<ServiceResponse<UserResponse>> RegisterUserAsync(RegisterRequest request);
    }
}
