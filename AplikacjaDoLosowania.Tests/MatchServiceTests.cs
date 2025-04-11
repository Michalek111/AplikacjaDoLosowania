using AplikacjaDoLosowania.Services.Implementation;

namespace AplikacjaDoLosowania.Tests
{
    public class MatchServiceTests
    {
        [Theory]
        [InlineData(-1, 13, false)]
        [InlineData(-1, -2, false)]
        [InlineData(13, 0, true)]
        [InlineData(13, 1, true)]
        [InlineData(13, 2, true)]
        [InlineData(13, 3, true)]
        [InlineData(13, 4, true)]
        [InlineData(13, 5, true)]
        [InlineData(13, 6, true)]
        [InlineData(13, 7, true)]
        [InlineData(13, 8, true)]
        [InlineData(13, 9, true)]
        [InlineData(13, 10, true)]
        [InlineData(13, 11, true)]
        [InlineData(13, 12, false)]
        [InlineData(13, 13, false)]
        [InlineData(0, 13, true)]
        [InlineData(1, 13, true)]
        [InlineData(2, 13, true)]
        [InlineData(3, 13, true)]
        [InlineData(4, 13, true)]
        [InlineData(5, 13, true)]
        [InlineData(6, 13, true)]
        [InlineData(7, 13, true)]
        [InlineData(8, 13, true)]
        [InlineData(9, 13, true)]
        [InlineData(10, 13, true)]
        [InlineData(11, 13, true)]
        [InlineData(12, 13, false)]
        [InlineData(-1, 0, false)]
        [InlineData(16, 12, true)]
        [InlineData(16, 13, true)]
        [InlineData(16, 14, true)]
        [InlineData(16, 15, false)]
        [InlineData(16, 16, false)]
        [InlineData(2, 16, false)]
        [InlineData(22, 6, false)]
        [InlineData(22, 22, false)]
        [InlineData(34, 31, true)]
        [InlineData(34, 32, true)]
        [InlineData(19, 15, true)]
        [InlineData(19, 16, true)]
        [InlineData(19, 17, true)]
        [InlineData(19, 18, false)]
        [InlineData(19, 19, false)]
        [InlineData(19, 11, false)]
        [InlineData(19, 12, false)]
        [InlineData(19, 13, false)]
        [InlineData(19, 14, false)]

        public void IsValidCs2Score_ShouldReturnExpected(int Team1Score, int Team2Score,bool expected)
        {
            var service = new MatchService(null!, null!);

            var result = service.IsValidCs2Score(Team1Score,Team2Score);

            Assert.Equal(expected, result); 
        }
    }
}