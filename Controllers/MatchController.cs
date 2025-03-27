using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Controllers
{

    public class MatchController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IMatchService _matchService;

        public MatchController(ApplicationDBContext dbContext, IMatchService matchService)
        {
            _dbContext = dbContext;
            _matchService = matchService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MatchHistory()
        {
            var matches = await _matchService.GetMatchHistoryAsync();
            return View(matches);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditMatch(int id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if (match == null) return NotFound();

            var team1 = await _dbContext.Players
                .Where(p => match.Team1Players.Contains(p.Nick))
                .ToListAsync();

            var team2 = await _dbContext.Players
                .Where(p => match.Team2Players.Contains(p.Nick))
                .ToListAsync();

            ViewBag.Team1 = team1;
            ViewBag.Team2 = team2;
            ViewBag.Match = match;

            return View(match);
        }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> EditMatch([FromBody] MatchData matchData)
            {
                Console.WriteLine("Edytowanie meczu...");
                Console.WriteLine($"Team1: {string.Join(",", matchData.Team1Ids)}, Team2: {string.Join(",", matchData.Team2Ids)}");
                Console.WriteLine($"Wynik: {matchData.Team1Score}:{matchData.Team2Score}, Mapa: {matchData.Map}");

                var result = await _matchService.EditMatchWithPlayersAsync(matchData);

                return Json(new
                {
                    success = result,
                    message = result ? "Zapisano zmiany!" : "Niepoprawne dane!"
                });

            }

        
    }
}
