using AplikacjaDoLosowania.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Repositories;
using AplikacjaDoLosowania.Services;

namespace AplikacjaDoLosowania.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerService _playerService;
        private readonly IMatchService _matchService;
        private readonly PredictionService _predictionService;

        public PlayersController(IPlayerService playerService, IPlayerRepository playerRepository, IMatchService matchService, PredictionService predictionService)
        {
            _playerRepository = playerRepository;
            _playerService = playerService;
            _matchService = matchService;
            _predictionService = predictionService;
        }


        public async Task<IActionResult> SelectPlayers()
        {
            ViewBag.SelectedPlayers = _playerService.GetSelectedPlayers(HttpContext.Session);
            var availablePlayers = await _playerService.GetAvailablePlayersAsync(HttpContext.Session);
            return View(availablePlayers);

        }


        [HttpPost]
        public async Task<IActionResult> AddToSelected(int id)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player != null)
            {
                _playerService.AddToSelected(HttpContext.Session, player);
            }
            return RedirectToAction("SelectPlayers");
        }


        [HttpPost]
        public IActionResult RemoveFromSelected(int id)
        {
            _playerService.RemoveFromSelected(HttpContext.Session, id);
            return RedirectToAction("SelectPlayers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if(string.IsNullOrWhiteSpace(player.Nick))
            {
                ModelState.AddModelError("Nick", "Pole jest wymagane");
                return View(player);
            }

            if(await _playerRepository.PlayerExistsAsync(player.Nick))
            {
                ModelState.AddModelError("Nick", "Gracz o takim nicku juz istnieje");
                return View(player);
            }

            player.GamesPlayed = 0;
            player.GamesWon = 0;

            await _playerRepository.AddPlayerAsync(player);
            await _playerRepository.SaveChangesAsync();

            return RedirectToAction("SelectPlayers");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RandomTeams()
        {
            var teams = _playerService.GenerateRandomTeams(HttpContext.Session);
            if (teams == null)
            {
                TempData["ErrorMessage"] = "Musisz wybrać 10 graczy!";
                return RedirectToAction("SelectPlayers");
            }

            ViewBag.Team1 = teams.Value.Team1;
            ViewBag.Team2 = teams.Value.Team2;

            float team1WinRatio = teams.Value.Team1.Average(p => p.GamesPlayed == 0 ? 0 : (float)p.GamesWon / p.GamesPlayed);
            float team2WinRatio = teams.Value.Team2.Average(p => p.GamesPlayed == 0 ? 0 : (float)p.GamesWon / p.GamesPlayed);

            float winProbability = _predictionService.PredictWinChance(team1WinRatio, team2WinRatio);

            ViewBag.Team1WinProbability = winProbability * 100;
            ViewBag.Team2WinProbability = (1 - winProbability) * 100;

            return View("RandomTeams");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmMatch([FromBody] MatchData matchData)
        {
            bool result = await _matchService.ConfirmMatchAsync(matchData);
            return Json(new { success = result, message = result ? "Mecz zatwierdzony!" : "Niepoprawny wynik meczu!" });
        }

        public async Task<IActionResult> PlayerStats()
        {
            var players = await _matchService.GetPlayerStatsAsync();
            return View(players);
        }

    }
}