using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Repositories.Implementation
{
    public class PlayerRepository : IPlayerRepository
    {

        private readonly ApplicationDBContext _dbContext;
        public PlayerRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<Player>> GetAllPlayersAsync() =>
            await _dbContext.Players.ToListAsync();


        public async Task<Player?> GetPlayerByIdAsync(int id) =>
            await _dbContext.Players.FindAsync(id);


        public async Task<bool> PlayerExistsAsync(string Nick) =>
            await _dbContext.Players.AnyAsync(p => p.Nick == Nick);

        public async Task AddPlayerAsync(Player player)
        {
            _dbContext.Players.Add(player);
        }

        public async Task SaveChangesAsync()
            => await _dbContext.SaveChangesAsync();

        public async Task<List<Player>> GetPlayersByIdsAsync(List<int> playerIds) =>
            await _dbContext.Players.Where(p => playerIds.Contains(p.Id)).ToListAsync();

        public async Task<List<Player>> GetPlayersByNicksAsync(List<string> nicks) =>
             await _dbContext.Players
                .Where(p => nicks.Contains(p.Nick))
                .ToListAsync();

        public async Task<List<Player>> GetAllPlayersOrderedAsync() =>
            await _dbContext.Players
                .OrderByDescending(p => p.GamesWon)
                .ThenByDescending(p => p.GamesPlayed)
                .ToListAsync();

    }
}
