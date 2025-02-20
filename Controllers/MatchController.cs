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

        public MatchController(ApplicationDBContext dbContext,IMatchService matchService)
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

            return View(match);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditMatch(Match match)
        {
            var result = await _matchService.UpdateMatchAsync(match);

            if (!result)
            {
                ViewBag.Error = "Niepoprawny wynik meczu! Sprawdź zasady CS2.";
                return View(match);
            }

            return RedirectToAction("MatchHistory");
        }

    }

}
