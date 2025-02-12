using System.ComponentModel.DataAnnotations;

namespace AplikacjaDoLosowania.Models
{
    public class Player
    {
        public int ID {  get; set; }

        [Required]
        [StringLength(100)]

        public string Nick { get; set; }


        [Range(0, int.MaxValue)]
        public int GamesPlayed { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int GamesWon { get; set; } = 0;
    }
}
