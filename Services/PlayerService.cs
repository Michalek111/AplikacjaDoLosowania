using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories;
using System.Net.NetworkInformation;

namespace AplikacjaDoLosowania.Services
{
    public class PlayerService : IPlayerService
    {
        private static List<Player> selectedPlayers = new List<Player>();
        private readonly IPlayerRepository _playerRepository;


        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<List<Player>> GetAvailablePlayersAsync()
        {
            var allPlayers = await _playerRepository.GetAllPlayersAsync();
            return allPlayers.Except(selectedPlayers).ToList();
        }

        public void AddToSelected(Player player)
        {
            if (selectedPlayers.Count < 10 && !selectedPlayers.Any(p => p.Id == player.Id))
            {
                selectedPlayers.Add(player);
            }
        }

        public void RemoveFromSelected(int id)
        {
            selectedPlayers.RemoveAll(p => p.Id == id);
        }

        public List<Player> GetSelectedPlayers()
        {
            return selectedPlayers;
        }
    }
}
