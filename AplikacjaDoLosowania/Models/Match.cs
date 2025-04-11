using System.ComponentModel.DataAnnotations;

namespace AplikacjaDoLosowania.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Team1Players { get; set; }

        [Required]
        public string Team2Players { get; set;}

        [Required]
        public int Team1Score { get; set; }

        [Required]
        public int Team2Score { get; set; }

        [Required]
        public string Map { get; set; }

        [Required]
        public DateTime MatchDate { get; set; } = DateTime.Now;
    }
}
