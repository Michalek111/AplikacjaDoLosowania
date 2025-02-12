using System.ComponentModel.DataAnnotations;

namespace AplikacjaDoLosowania.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole Nick jest wymagane.")]
        [StringLength(100, ErrorMessage = "Nick może mieć maksymalnie 100 znaków.")]
        public string Nick { get; set; } = string.Empty;

        public int GamesPlayed { get; set; } = 0;
        public int GamesWon { get; set; } = 0;
    }
}
