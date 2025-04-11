namespace AplikacjaDoLosowania.Services.Interface
{
    public interface IPredictionService
    {
        float PredictWinChance(float team1WinRatio, float team2WinRatio);
    }
}
