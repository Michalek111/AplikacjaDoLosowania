using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Services.Interface
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAvailablePlayersAsync(ISession session);
        void AddToSelected(ISession session,Player player);
        void RemoveFromSelected(ISession session, int id);
        List<Player> GetSelectedPlayers(ISession session);

        (List<Player> Team1, List<Player> Team2)? GenerateRandomTeams(ISession session);

        Task AddPlayerToSelectedAsync(ISession session, int playerId);

        Task<(bool Success, string? ErrorMessage)> TryCreatePlayerAsync(Player player);

        Task<List<Player>> GetPlayersByIdsAsync(List<int> ids);
        Task<List<Player>> GetPlayersByNicksAsync(List<string> nicks);
        Task<List<Player>> GetAllPlayersOrderedAsync();



    }
}
