using FileServer.Server.Models.Entities;

namespace FileServer.Server.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> AddAccountAsync(Account account);
        Task<Account?> GetAccountByGuidAsync(Guid guid);
        Task<Account?> GetAccountByNameAsync(string username);
    }
}
