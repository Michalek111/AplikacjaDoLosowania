using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAvailablePlayersAsync();
        void AddToSelected(Player player);
        void RemoveFromSelected(int id);
        List<Player> GetSelectedPlayers();


        (List<Player> Team1, List<Player> Team2)? GenerateRandomTeams();
    }
}
