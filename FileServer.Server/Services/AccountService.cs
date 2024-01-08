using FileServer.Models;
using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using FileServer.Server.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileServer.Server.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IOptions<JwtSettings> _jwtOptions;

        public AccountService(IAccountRepository accountRepository, IOptions<JwtSettings> options)
        {
            _accountRepository = accountRepository;
            _jwtOptions = options;
        }

        public async Task<ServiceResponse<UserResponse>> RegisterUserAsync(RegisterRequest request)
        {
            if (request.Password != request.RepeatedPassword)
                return ServiceResponse<UserResponse>.Reject("Passwords do not match.");

            Account? existingAccount = await _accountRepository.GetAccountByNameAsync(request.UserName);
            if (existingAccount is not null)
                return ServiceResponse<UserResponse>.Reject("Username is taken.");

            string encryptedPassword = EncryptPassword(request.Password);

            Account account = new Account()
            {
                Guid = Guid.NewGuid(),
                UserName = request.UserName,
                Password = encryptedPassword,
                Type = UserType.User,
                Approved = false
            };

            await _accountRepository.AddAccountAsync(account);

            return ServiceResponse<UserResponse>.Accept(new UserResponse()
            {
                Guid = account.Guid,
                UserName = account.UserName,
                Type = UserType.User
            });
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginRequest request)
        {
            Account? account = await _accountRepository.GetAccountByNameAsync(request.UserName);

            if (account is null || !BCrypt.Net.BCrypt.Verify(request.Password, account.Password))
                return ServiceResponse<string>.Reject("No account with these details exists.");

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] 
                {
                    new Claim("Id", account.Guid.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, account.UserName),
                    new Claim("Type", account.Type.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = _jwtOptions.Value.Issuer,
                Audience = _jwtOptions.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Value.Key)), SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            SecurityToken token = handler.CreateToken(tokenDescriptor);
            return ServiceResponse<string>.Accept(handler.WriteToken(token));
        }

        private string EncryptPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
