using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Services
{
    public class MatchService : IMatchService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ApplicationDBContext _dbContext;

        public MatchService(IPlayerRepository playerRepository, ApplicationDBContext dbContext)
        {
            _playerRepository = playerRepository;
            _dbContext = dbContext;
        }

        public bool IsValidCs2Score(int team1Score, int team2Score)
        {
            Console.WriteLine($"🔍 Sprawdzanie wyniku: Team1 - {team1Score}, Team2 - {team2Score}");

            if ((team1Score >= 13 || team2Score >= 13) && Math.Abs(team1Score - team2Score) >= 2)
            {
                Console.WriteLine("✅ Mecz zakończony przy 13 zwycięstwach!");
                return true;
            }

            int winningScore = 16;
            while (winningScore <= 100)
            {
                if ((team1Score == winningScore && team2Score >= winningScore - 4 && team2Score <= winningScore - 2) ||
                    (team2Score == winningScore && team1Score >= winningScore - 4 && team1Score <= winningScore - 2))
                {
                    Console.WriteLine($"✅ Mecz zakończony w dogrywce do {winningScore}!");
                    return true;
                }

                if (team1Score == team2Score && team1Score == winningScore - 1)
                {
                    Console.WriteLine($"🔁 Dogrywka! Kolejna runda do {winningScore + 3}");
                    winningScore += 3;
                    continue;
                }

                break;
            }

            Console.WriteLine("❌ Wynik meczu nie spełnia zasad CS2!");
            return false;
        }

        public async Task<bool> ConfirmMatchAsync(MatchData matchData)
        {
            Console.WriteLine($"Otrzymano wynik: Team1 - {matchData.Team1Score}, Team2 - {matchData.Team2Score}");
            Console.WriteLine($"Otrzymano ID graczy: Team1 - {string.Join(",", matchData.Team1Ids)}, Team2 - {string.Join(",", matchData.Team2Ids)}");

            if (!IsValidCs2Score(matchData.Team1Score, matchData.Team2Score))
            {
                Console.WriteLine("❌ Wynik meczu nie spełnia zasad CS2!");
                return false;
            }

            var playerIds = matchData.Team1Ids.Concat(matchData.Team2Ids).ToList();
            var players = await _playerRepository.GetPlayersByIdsAsync(playerIds);

            foreach (var player in players)
            {
                player.GamesPlayed += 1;
            }

            if (matchData.Team1Score > matchData.Team2Score)
            {
                foreach (var player in players.Where(p => matchData.Team1Ids.Contains(p.Id)))
                {
                    player.GamesWon += 1;
                }
            }
            else if (matchData.Team2Score > matchData.Team1Score)
            {
                foreach (var player in players.Where(p => matchData.Team2Ids.Contains(p.Id)))
                {
                    player.GamesWon += 1;
                }
            }

            Match newMatch = new Match
            {
                Team1Players = string.Join(", ", players.Where(p => matchData.Team1Ids.Contains(p.Id)).Select(p => p.Nick)),
                Team2Players = string.Join(", ", players.Where(p => matchData.Team2Ids.Contains(p.Id)).Select(p => p.Nick)),
                Team1Score = matchData.Team1Score,
                Team2Score = matchData.Team2Score,
                Map = matchData.Map,
                MatchDate = DateTime.Now
            };

            _dbContext.Matches.Add(newMatch);
            await _dbContext.SaveChangesAsync();

            Console.WriteLine("✅ Mecz zatwierdzony!");
            return true;
        }

        public async Task<List<Player>> GetPlayerStatsAsync()
        {
            return await _dbContext.Players
                .OrderByDescending(p => p.GamesWon)
                .ThenByDescending(p => p.GamesPlayed)
                .ToListAsync();
        }
        /*
        public async Task<bool> UpdateMatchAsync(Match match)
        {
            var existingMatch = await _dbContext.Matches.FindAsync(match.Id);
            if (existingMatch == null) return false;


            if (!IsValidCs2Score(match.Team1Score, match.Team2Score))
            {
                return false;
            }


            var team1Players = await _dbContext.Players
                .Where(p => existingMatch.Team1Players.Contains(p.Nick))
                .ToListAsync();

            var team2Players = await _dbContext.Players
                .Where(p => existingMatch.Team2Players.Contains(p.Nick))
                .ToListAsync();


            foreach (var player in team1Players.Concat(team2Players))
            {
                player.GamesPlayed -= 1;
                if ((existingMatch.Team1Score > existingMatch.Team2Score && team1Players.Contains(player)) ||
                    (existingMatch.Team2Score > existingMatch.Team1Score && team2Players.Contains(player)))
                {
                    player.GamesWon -= 1;
                }
            }


            existingMatch.Team1Score = match.Team1Score;
            existingMatch.Team2Score = match.Team2Score;


            foreach (var player in team1Players.Concat(team2Players))
            {
                player.GamesPlayed += 1;
                if ((match.Team1Score > match.Team2Score && team1Players.Contains(player)) ||
                    (match.Team2Score > match.Team1Score && team2Players.Contains(player)))
                {
                    player.GamesWon += 1;
                }
            }


            await _dbContext.SaveChangesAsync();
            return true;
        }
        */
        public async Task<List<Match>> GetMatchHistoryAsync()
        {
            return await _dbContext.Matches
                .OrderByDescending(m => m.MatchDate)
                .ToListAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _dbContext.Matches.FindAsync(id);
        }

        public async Task<bool> EditMatchWithPlayersAsync(MatchData matchData)
        {
            var match = await _dbContext.Matches.FindAsync(matchData.MatchId);
            if (match == null) return false;

            if (!IsValidCs2Score(matchData.Team1Score, matchData.Team2Score))
            {
                return false;
            }

           
            var oldTeam1 = await _dbContext.Players
                .Where(p => match.Team1Players.Contains(p.Nick)).ToListAsync();
            var oldTeam2 = await _dbContext.Players
                .Where(p => match.Team2Players.Contains(p.Nick)).ToListAsync();

            foreach (var player in oldTeam1.Concat(oldTeam2))
            {
                player.GamesPlayed--;
                if ((match.Team1Score > match.Team2Score && oldTeam1.Contains(player)) ||
                    (match.Team2Score > match.Team1Score && oldTeam2.Contains(player)))
                {
                    player.GamesWon--;
                }
            }

            
            var newTeam1 = await _dbContext.Players
                .Where(p => matchData.Team1Ids.Contains(p.Id)).ToListAsync();
            var newTeam2 = await _dbContext.Players
                .Where(p => matchData.Team2Ids.Contains(p.Id)).ToListAsync();

            match.Team1Players = string.Join(", ", newTeam1.Select(p => p.Nick));
            match.Team2Players = string.Join(", ", newTeam2.Select(p => p.Nick));
            match.Team1Score = matchData.Team1Score;
            match.Team2Score = matchData.Team2Score;
            match.Map = matchData.Map;
            match.MatchDate = DateTime.Now;

            
            foreach (var player in newTeam1.Concat(newTeam2))
            {
                player.GamesPlayed++;
                if ((match.Team1Score > match.Team2Score && newTeam1.Contains(player)) ||
                    (match.Team2Score > match.Team1Score && newTeam2.Contains(player)))
                {
                    player.GamesWon++;
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }


    }
}
