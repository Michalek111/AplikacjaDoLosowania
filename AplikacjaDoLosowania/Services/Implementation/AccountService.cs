using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using AplikacjaDoLosowania.Services.Interface;

namespace AplikacjaDoLosowania.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _accountRepository.GetUserByNameAsync(username);
            if (user == null) return null;

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.passwordHash);
            return isPasswordCorrect ? user : null;
        }

        public async Task<(bool success, string? message)> RegisterUserAsync(string username, string password)
        {
            if (await _accountRepository.UserExistsAsync(username))
                return (false, "Użytkownik już istnieje!");

            var hashed = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                Name = username,
                passwordHash = hashed,
                Role = "User"
            };

            await _accountRepository.AddUserAsync(newUser);
            await _accountRepository.SaveChangesAsync();

            return (true, null);
        }
    }
}
