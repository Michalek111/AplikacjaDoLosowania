using AplikacjaDoLosowania.DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AplikacjaDoLosowania.Controllers
{
    public class MapController : Controller
    {
        private readonly ApplicationDBContext _dbContext;

        public MapController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> MapHistory()
        {
            var matches = await _dbContext.Matches
                .OrderByDescending(m => m.MatchDate) 
                .ToListAsync();

            return View(matches);
        }
    }
}
