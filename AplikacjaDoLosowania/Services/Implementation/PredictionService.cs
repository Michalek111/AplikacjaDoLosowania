using AplikacjaDoLosowania.DataBase;
using AplikacjaDoLosowania.Models;
using AplikacjaDoLosowania.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace AplikacjaDoLosowania.Services.Implementation
{
    public class PredictionService : IPredictionService
    {
        private readonly MLContext _mlContext;
        private PredictionEngine<MatchData, MatchPrediction>? _predictionEngine;

        public PredictionService(ApplicationDBContext dbContext)
        {
            _mlContext = new MLContext();
            TrainModel(dbContext);
        }

        private void TrainModel(ApplicationDBContext dbContext)
        {
            var matches = dbContext.Matches.ToList();

            if (!matches.Any())
            {
                _predictionEngine = null;
                return;
            }

            var data = matches.Select(m => new MatchData
            {
                Team1WinRatio = m.Team1Players.Split(',').Select(p => dbContext.Players.FirstOrDefault(pl => pl.Nick == p.Trim())?.WinRatio ?? 0).Average(),
                Team2WinRatio = m.Team2Players.Split(',').Select(p => dbContext.Players.FirstOrDefault(pl => pl.Nick == p.Trim())?.WinRatio ?? 0).Average(),
                Winner = m.Team1Score > m.Team2Score
            });

            var trainData = _mlContext.Data.LoadFromEnumerable(data);
            var pipeline = _mlContext.Transforms.Concatenate("Features", new[] { "Team1WinRatio", "Team2WinRatio" })
                .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Winner"));

            var model = pipeline.Fit(trainData);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<MatchData, MatchPrediction>(model);
        }

        public float PredictWinChance(float team1WinRatio, float team2WinRatio)
        {
            if (_predictionEngine == null) return 0.0f;

            var prediction = _predictionEngine.Predict(new MatchData
            {
                Team1WinRatio = team1WinRatio,
                Team2WinRatio = team2WinRatio
            });

            return prediction.Probability;
        }
    }
}
