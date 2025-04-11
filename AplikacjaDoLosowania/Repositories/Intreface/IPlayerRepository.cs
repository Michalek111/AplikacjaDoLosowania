using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Repositories.Intreface
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player?> GetPlayerByIdAsync(int id);
        Task<bool> PlayerExistsAsync(string Nick);
        Task AddPlayerAsync(Player player);
        Task SaveChangesAsync();

        Task<List<Player>> GetPlayersByIdsAsync(List<int> playersIds);

        Task<List<Player>> GetPlayersByNicksAsync(List<string> nicks);
        Task<List<Player>> GetAllPlayersOrderedAsync();

    }
}
