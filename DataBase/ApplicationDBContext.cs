using AplikacjaDoLosowania.Models;
using Microsoft.EntityFrameworkCore;


namespace AplikacjaDoLosowania.DataBase
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Player>  Players { get; set; }
        public DbSet<Match> Matches { get; set; }   

        public DbSet<User> Users { get; set; }
    }
}
