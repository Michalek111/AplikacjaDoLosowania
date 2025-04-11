using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AplikacjaDoLosowania.Models
{
    public class User
    {
        [Key]
        public int Id {  get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string passwordHash { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
