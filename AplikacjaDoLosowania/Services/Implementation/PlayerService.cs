using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using AplikacjaDoLosowania.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

namespace AplikacjaDoLosowania.Services.Implementation
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
            return await _playerRepository.GetAllPlayersAsync();
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

        public async Task AddPlayerToSelectedAsync(ISession session, int playerId)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(playerId);
            if (player != null)
            {
                AddToSelected(session, player);
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> TryCreatePlayerAsync(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Nick))
                return (false, "Pole jest wymagane");

            if (await _playerRepository.PlayerExistsAsync(player.Nick))
                return (false, "Gracz o takim nicku już istnieje");

            player.GamesPlayed = 0;
            player.GamesWon = 0;

            await _playerRepository.AddPlayerAsync(player);
            await _playerRepository.SaveChangesAsync();

            return (true, null);
        }

        public async Task<List<Player>> GetPlayersByIdsAsync(List<int> ids)
        {
            return await _playerRepository.GetPlayersByIdsAsync(ids);
        }

        public async Task<List<Player>> GetPlayersByNicksAsync(List<string> nicks)
        {
            return await _playerRepository.GetPlayersByNicksAsync(nicks);
        }

        public async Task<List<Player>> GetAllPlayersOrderedAsync()
        {
            return await _playerRepository.GetAllPlayersOrderedAsync();
        }

    }
}
