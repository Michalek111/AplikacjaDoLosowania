namespace AplikacjaDoLosowania.Models
{
    public class MatchData
    {
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
        public List<int> Team1Ids { get; set; }
        public List<int> Team2Ids { get; set; }

        public string Map { get; set; }
    }
}
