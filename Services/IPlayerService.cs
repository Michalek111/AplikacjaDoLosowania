using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Services
{
    public interface IPlayerService
    {
        Task<List<Player>> GetAvailablePlayersAsync(ISession session);
        void AddToSelected(ISession session,Player player);
        void RemoveFromSelected(ISession session, int id);
        List<Player> GetSelectedPlayers(ISession session);

        (List<Player> Team1, List<Player> Team2)? GenerateRandomTeams(ISession session);


    }
}
