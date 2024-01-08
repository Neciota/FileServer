using FileServer.Server.Data;
using FileServer.Server.Models.Entities;
using FileServer.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileServer.Server.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<Account?> GetAccountByNameAsync(string username)
        {
            return await _context.Accounts.FirstOrDefaultAsync(account => account.UserName == username);
        }

        public async Task<Account?> GetAccountByGuidAsync(Guid guid)
        {
            return await _context.Accounts.FirstOrDefaultAsync(account => account.Guid == guid);
        }
    }
}
