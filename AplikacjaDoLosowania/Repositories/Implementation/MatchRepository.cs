using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Repositories.Implementation
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public MatchRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _dbContext.Matches.FindAsync(id);
        }

        public async Task<List<Match>> GetAllMatchesAsync()
        {
            return await _dbContext.Matches
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task AddMatchAsync(Match match)
        {
            await _dbContext.Matches.AddAsync(match);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
