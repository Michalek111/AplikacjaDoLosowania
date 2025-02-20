using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Services
{
    public interface IMatchService
    {
        bool IsValidCs2Score(int team1Score, int team2Score);
        Task<bool> ConfirmMatchAsync(MatchData matchData);
        Task<List<Player>> GetPlayerStatsAsync();
    }

}
