using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories.Intreface;
using AplikacjaDoLosowania.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Services.Implementation
{
    public class MatchService : IMatchService
    {
        private readonly IPlayerService _playerService;
        private readonly IMatchRepository _matchRepository;


        public MatchService(IPlayerService playerService, IMatchRepository matchRepository)
        {
            _playerService = playerService;
            _matchRepository = matchRepository;
        }

         public bool IsValidCs2Score(int team1Score, int team2Score)
         {

             int lbDogrywek = 0;
             int totalRounds = team1Score + team2Score;

             if (team1Score < 0 || team2Score < 0)
             {
                 return false;

             }


             if ((team1Score == 13 || team2Score == 13) && totalRounds <= 24)
             {
                 return true;
             }

             if ((totalRounds - 24) % 6 == 0)
             {
             lbDogrywek = (totalRounds - 24) / 6;
             }
             else
             {
                 lbDogrywek = ((totalRounds - 24) / 6) +1;
             }

            int winningScore = Math.Max(team1Score, team2Score);
            int losingScore = Math.Min(team1Score, team2Score);

            if (winningScore - losingScore > 4)
            {
                return false;
            }

            if ( (team1Score == 12 + (3 * lbDogrywek) + 1) || (team2Score == 12 + (3 * lbDogrywek) + 1) && (team1Score + team2Score <= 24 + (lbDogrywek * 6)))
            {
                return true;
            }

             return false;
         }

        public async Task<bool> ConfirmMatchAsync(MatchData matchData)
        {
            if (!IsValidCs2Score(matchData.Team1Score, matchData.Team2Score))
                return false;

            var playerIds = matchData.Team1Ids.Concat(matchData.Team2Ids).ToList();
            var players = await _playerService.GetPlayersByIdsAsync(playerIds);

            foreach (var player in players)
                player.GamesPlayed++;

            if (matchData.Team1Score > matchData.Team2Score)
            {
                foreach (var player in players.Where(p => matchData.Team1Ids.Contains(p.Id)))
                    player.GamesWon++;
            }
            else
            {
                foreach (var player in players.Where(p => matchData.Team2Ids.Contains(p.Id)))
                    player.GamesWon++;
            }

            var newMatch = new Match
            {
                Team1Players = string.Join(", ", players.Where(p => matchData.Team1Ids.Contains(p.Id)).Select(p => p.Nick)),
                Team2Players = string.Join(", ", players.Where(p => matchData.Team2Ids.Contains(p.Id)).Select(p => p.Nick)),
                Team1Score = matchData.Team1Score,
                Team2Score = matchData.Team2Score,
                Map = matchData.Map,
            };

            await _matchRepository.AddMatchAsync(newMatch);
            await _matchRepository.SaveChangesAsync();
            return true;
        }


        public async Task<List<Player>> GetPlayerStatsAsync()
        {
            return await _playerService.GetAllPlayersOrderedAsync();
        }



        public async Task<List<Match>> GetMatchHistoryAsync()
        {
            return await _matchRepository.GetAllMatchesAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _matchRepository.GetMatchByIdAsync(id);
        }

        public async Task<bool> EditMatchWithPlayersAsync(MatchData matchData)
        {
            var match = await _matchRepository.GetMatchByIdAsync(matchData.MatchId);
            if (match == null || !IsValidCs2Score(matchData.Team1Score, matchData.Team2Score))
                return false;

            var oldPlayers = await _playerService.GetPlayersByNicksAsync(
                match.Team1Players.Split(", ").Concat(match.Team2Players.Split(", ")).ToList());

            foreach (var p in oldPlayers)
            {
                p.GamesPlayed--;
                if ((match.Team1Score > match.Team2Score && match.Team1Players.Contains(p.Nick)) ||
                    (match.Team2Score > match.Team1Score && match.Team2Players.Contains(p.Nick)))
                {
                    p.GamesWon--;
                }
            }

            var newTeam1 = await _playerService.GetPlayersByIdsAsync(matchData.Team1Ids.ToList());
            var newTeam2 = await _playerService.GetPlayersByIdsAsync(matchData.Team2Ids.ToList());


            match.Team1Players = string.Join(", ", newTeam1.Select(p => p.Nick));
            match.Team2Players = string.Join(", ", newTeam2.Select(p => p.Nick));
            match.Team1Score = matchData.Team1Score;
            match.Team2Score = matchData.Team2Score;
            match.Map = matchData.Map;
            match.MatchDate = DateTime.Now;

            foreach (var p in newTeam1.Concat(newTeam2))
            {
                p.GamesPlayed++;
                if ((match.Team1Score > match.Team2Score && newTeam1.Contains(p)) ||
                    (match.Team2Score > match.Team1Score && newTeam2.Contains(p)))
                {
                    p.GamesWon++;
                }
            }

            await _matchRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<Player>> GetPlayersByNicksAsync(List<string> nicks)
        {
            return await _playerService.GetPlayersByNicksAsync(nicks);
        }



    }
}
