using FileServer.Models.Responses;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using FileServer.Server.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace FileServer.Server.Utilities
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAccountRepository accountRepository, IUserRequestService userRequestService)
        {
            // Extract the bearer token from the request headers
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            if (!string.IsNullOrEmpty(token))
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken decodedToken = handler.ReadToken(token) as JwtSecurityToken ?? throw new ArgumentException("Invalid JWT");
                Guid guid = Guid.Parse(decodedToken.Claims.First(claim => claim.Type == "Id").Value);

                Account? account = await accountRepository.GetAccountByGuidAsync(guid);
                userRequestService.User = account is null ? null : new UserResponse()
                {
                    Guid = account.Guid,
                    UserName = account.UserName,
                    Type = account.Type
                };
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
