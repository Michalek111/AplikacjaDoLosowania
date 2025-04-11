using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Controllers
{

    public class MatchController : Controller
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
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
            if (match == null)
            {
                return NotFound();
            }

            var team1Nicks = match.Team1Players.Split(", ").ToList();
            var team2Nicks = match.Team2Players.Split(", ").ToList();

            var team1 = await _matchService.GetPlayersByNicksAsync(team1Nicks);
            var team2 = await _matchService.GetPlayersByNicksAsync(team2Nicks);

            ViewBag.Team1 = team1;
            ViewBag.Team2 = team2;
            ViewBag.Match = match;

            return View(match);
        }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> EditMatch([FromBody] MatchData matchData)
            {

                var result = await _matchService.EditMatchWithPlayersAsync(matchData);

                return Json(new
                {
                    success = result,
                    message = result ? "Zapisano zmiany!" : "Niepoprawne dane!"
                });

            }

        
    }
}
