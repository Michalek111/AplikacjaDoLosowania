using Microsoft.ML.Data;

namespace AplikacjaDoLosowania.Models
{
    public class MatchData
    {
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public int[] Team1Ids { get; set; } = new int[0];
        public int[] Team2Ids { get; set; } = new int[0];

        public int MatchId { get; set; }
        public string Map { get; set; }

        [LoadColumn(0)] public float Team1WinRatio { get; set; }
        [LoadColumn(1)] public float Team2WinRatio { get; set; }
        [LoadColumn(2)] public bool Winner { get; set; }
    }
}
