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

        public PlayersController(IPlayerService playerService, IPlayerRepository playerRepository, IMatchService matchService)
        {
            _playerRepository = playerRepository;
            _playerService = playerService;
            _matchService = matchService;
        }


        public async Task<IActionResult> SelectPlayers()
        {
            ViewBag.SelectedPlayers = _playerService.GetSelectedPlayers();
            var availablePlayers = await _playerService.GetAvailablePlayersAsync();

            return View(availablePlayers);

        }


        [HttpPost]
        public async Task<IActionResult> AddToSelected(int id)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);

            if (player != null)
            {
                _playerService.AddToSelected(player);
            }
            return RedirectToAction("SelectPlayers");
        }


        [HttpPost]
        public IActionResult RemoveFromSelected(int id)
        {
            _playerService.RemoveFromSelected(id);
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

            var selectedPlayers = _playerService.GetSelectedPlayers();

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