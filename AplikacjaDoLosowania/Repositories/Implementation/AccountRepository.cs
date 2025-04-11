using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AplikacjaDoLosowania.Repositories.Implementation
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public AccountRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserByNameAsync(string username)
            => await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == username);

        public async Task<bool> UserExistsAsync(string username)
            => await _dbContext.Users.AnyAsync(u => u.Name == username);

        public async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
