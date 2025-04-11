using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Repositories.Intreface
{
    public interface IAccountRepository
    {
        Task<User?> GetUserByNameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
