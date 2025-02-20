using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace AplikacjaDoLosowania.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private const string SessionKey = "SelectedPlayers";


        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }
        private List<Player> GetSessionPlayers(ISession session)
        {
            var sessionData = session.GetString(SessionKey);
            return sessionData != null ? JsonConvert.DeserializeObject<List<Player>>(sessionData) : new List<Player>();
        }

        private void SaveSessionPlayers(ISession session, List<Player> players)
        {
            session.SetString(SessionKey, JsonConvert.SerializeObject(players));
        }

        public async Task<List<Player>> GetAvailablePlayersAsync(ISession session)
        {
            var allPlayers = await _playerRepository.GetAllPlayersAsync();
            var selectedPlayers = GetSessionPlayers(session);
            return allPlayers.Except(selectedPlayers).ToList();
        }

        public void AddToSelected(ISession session, Player player)
        {
            var selectedPlayers = GetSessionPlayers(session);
            if (selectedPlayers.Count < 10 && !selectedPlayers.Any(p => p.Id == player.Id))
            {
                selectedPlayers.Add(player);
                SaveSessionPlayers(session, selectedPlayers);
            }
        }

        public void RemoveFromSelected(ISession session, int id)
        {
            var selectedPlayers = GetSessionPlayers(session);
            selectedPlayers.RemoveAll(p => p.Id == id);
            SaveSessionPlayers(session, selectedPlayers);
        }

        public List<Player> GetSelectedPlayers(ISession session)
        {
            return GetSessionPlayers(session);
        }

        public (List<Player> Team1, List<Player> Team2)? GenerateRandomTeams(ISession session)
        {
            var selectedPlayers = GetSessionPlayers(session);
            if (selectedPlayers.Count != 10)
            {
                return null;
            }

            var shuffled = selectedPlayers.OrderBy(p => Guid.NewGuid()).ToList();
            var team1 = shuffled.Take(5).ToList();
            var team2 = shuffled.Skip(5).Take(5).ToList();

            return (team1, team2);
        }

    }
}
