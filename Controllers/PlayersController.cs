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
                var shuffled = selectedPlayers.OrderBy(p => System.Guid.NewGuid()).ToList();
                var team1 = shuffled.Take(5).ToList();
                var team2 = shuffled.Skip(5).Take(5).ToList();

                ViewBag.Team1 = team1;
                ViewBag.Team2 = team2;

                return View("RandomTeams");
            }
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

    }

}

    
