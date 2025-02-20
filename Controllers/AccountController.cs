using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;

namespace AplikacjaDoLosowania.Controllers
{
    public class AccountController : Controller
    {

        private readonly ApplicationDBContext _dbContext;
        public AccountController(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == username);

            if (user == null)
            {
                ViewBag.Error = "Użytkownik nie istnieje!";
                return View();
            }

            Console.WriteLine($"🔍 Sprawdzam login: {username}");
            Console.WriteLine($"🔑 Podane hasło: {password}");
            Console.WriteLine($"🛠 Zaszyfrowane w bazie: {user.passwordHash}");

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.passwordHash);

            Console.WriteLine($"✅ Hasło poprawne? {isPasswordCorrect}");

            if (!isPasswordCorrect)
            {
                ViewBag.Error = "Błędne dane logowania!";
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string username, string password)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Name == username))
            {
                ViewBag.Error = "Użytkownik już istnieje!";
                return View();
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            User newUser = new User
            {
                Name = username,
                passwordHash = hashedPassword,
                Role = "User"
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            ViewBag.Success = "Użytkownik dodany!";
            return View();
        }

    }
}
