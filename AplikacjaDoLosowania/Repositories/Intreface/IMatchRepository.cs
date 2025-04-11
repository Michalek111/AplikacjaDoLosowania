using AplikacjaDoLosowania.Models;

namespace AplikacjaDoLosowania.Repositories.Intreface
{
    public interface IMatchRepository
    {
        Task<Match?> GetMatchByIdAsync(int id);
        Task<List<Match>> GetAllMatchesAsync();
        Task AddMatchAsync(Match match);

        Task SaveChangesAsync();
    }
}
