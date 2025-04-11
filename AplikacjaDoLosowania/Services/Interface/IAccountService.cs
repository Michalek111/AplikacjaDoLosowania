using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Services.Interface
{
    public interface IAccountService
    {
        Task<User?> AuthenticateUserAsync(string username, string password);
        Task<(bool success, string? message)> RegisterUserAsync(string username, string password);
    }
}
