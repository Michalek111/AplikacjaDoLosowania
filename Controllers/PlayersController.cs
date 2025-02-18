using AplikacjaDoLosowania.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.DataBase;

namespace AplikacjaDoLosowania.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDBContext _dbContext;


        private static List<Player> selectedPlayers = new List<Player>();

        public PlayersController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IActionResult> SelectPlayers()
        {
            var allPlayers = await _dbContext.Players.ToListAsync();
            var availablePlayers = allPlayers.Except(selectedPlayers).ToList();

            ViewBag.SelectedPlayers = selectedPlayers;
            return View(availablePlayers);
        }


        [HttpPost]
        public IActionResult AddToSelected(int id)
        {
            var player = _dbContext.Players.Find(id);
            if (player != null && selectedPlayers.Count < 10 && !selectedPlayers.Any(p => p.Id == id))
            {
                selectedPlayers.Add(player);
            }
            return RedirectToAction("SelectPlayers");
        }


        [HttpPost]
        public IActionResult RemoveFromSelected(int id)
        {
            selectedPlayers.RemoveAll(p => p.Id == id);
            return RedirectToAction("SelectPlayers");
        }


        [HttpPost]
        public IActionResult RandomTeams()
        {
            if (selectedPlayers.Count == 10)
            {
                var shuffled = selectedPlayers.OrderBy(p => Guid.NewGuid()).ToList();
                var team1 = shuffled.Take(5).ToList();
                var team2 = shuffled.Skip(5).Take(5).ToList();

                ViewBag.Team1 = team1;
                ViewBag.Team2 = team2;

                return View("RandomTeams");
            }

            TempData["ErrorMessage"] = "Musisz wybrać 10 graczy!";
            return RedirectToAction("SelectPlayers");
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.Nick))
            {
                ModelState.AddModelError("Nick", "Pole Nick jest wymagane.");
                return View(player);
            }

            if (await _dbContext.Players.AnyAsync(p => p.Nick == player.Nick))
            {
                ModelState.AddModelError("Nick", "Gracz o tym nicku już istnieje.");
                return View(player);
            }

            player.GamesPlayed = 0;
            player.GamesWon = 0;

            _dbContext.Players.Add(player);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("SelectPlayers");
        }
        private bool IsValidCs2Score(int team1Score, int team2Score)
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
                if (team1Score == winningScore && team2Score >= winningScore - 4 && team2Score <= winningScore - 2)
                {
                    Console.WriteLine($"✅ Mecz zakończony w dogrywce do {winningScore}!");
                    return true;
                }
                if (team2Score == winningScore && team1Score >= winningScore - 4 && team1Score <= winningScore - 2)
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

        [HttpPost]
        public async Task<IActionResult> ConfirmMatch([FromBody] MatchData matchData)
        {
            Console.WriteLine($"Otrzymano wynik: Team1 - {matchData.Team1Score}, Team2 - {matchData.Team2Score}");
            Console.WriteLine($"Otrzymano ID graczy: Team1 - {string.Join(",", matchData.Team1Ids)}, Team2 - {string.Join(",", matchData.Team2Ids)}");

            if (!IsValidCs2Score(matchData.Team1Score, matchData.Team2Score))
            {
                Console.WriteLine("❌ Wynik meczu nie spełnia zasad CS2!");
                return Json(new { success = false, message = "Niepoprawny wynik meczu! Sprawdź zasady CS2." });
            }

            var playerIds = matchData.Team1Ids.Concat(matchData.Team2Ids).ToList();
            var players = await _dbContext.Players.Where(p => playerIds.Contains(p.Id)).ToListAsync();

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

            await _dbContext.SaveChangesAsync();

            Console.WriteLine("✅ Mecz zatwierdzony!");
            return Json(new { success = true, message = "Mecz zatwierdzony!" });
        }

        public class MatchData
        {
            public int Team1Score { get; set; }
            public int Team2Score { get; set; }
            public List<int> Team1Ids { get; set; }
            public List<int> Team2Ids { get; set; }
        }



    }
}