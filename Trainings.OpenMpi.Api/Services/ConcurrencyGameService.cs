using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Common.Enums;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Services
{
    public class ConcurrencyGameService
    {
        private readonly ConnectionStorage connectionStorage;
        private readonly TrainingMpiDbContext dbContext;
        private static readonly Random random = new Random();

        public ConcurrencyGameService(ConnectionStorage connectionStorage, TrainingMpiDbContext dbContext)
        {
            this.connectionStorage = connectionStorage;
            this.dbContext = dbContext;
        }

        public async Task<long?> SetValueGetNextAsync(int userId, long number)
        {
            var gameQuery = from cg in dbContext.ConcurrencyGames
                            join g in dbContext.Games on cg.GameId equals g.Id
                            join r in dbContext.ConcurrencyGameRounds on cg.GameId equals r.ConcurrencyGameId into Rounds
                            from r in Rounds.DefaultIfEmpty()
                            orderby r.OrderIndex
                            where r.UserId == userId && !r.IsFinished && g.IsActive
                            select new
                            {
                                Round = r,
                                Game = cg
                            };
            var gameData = await gameQuery.Take(2).ToArrayAsync();

            if (!gameData.Any())
                throw new ArgumentException();

            var game = gameData.First().Game;
            var finishedRound = gameData.First().Round;

            await Task.Delay(random.Next(1000, 3000));

            game.CurrentSum = number;
            finishedRound.IsFinished = true;
            dbContext.Update(game);
            dbContext.Update(finishedRound);
            await dbContext.SaveChangesAsync();

            if (gameData.Length == 1)
                return null;

            return gameData[1].Round.AddCoeff;
        }

        public async Task<Game> StartGameAsync()
        {
            var count = connectionStorage.PlayersCount;

            var rounds = connectionStorage
                .GetIds()
                .SelectMany(GenerateRounds)
                .ToList();

            var game = new Game()
            {
                IsActive = true,
                Type = GameType.Concurrency,
                ConcurrencyGame = new ConcurrencyGame()
                {
                    ConcurrencyGameRounds = rounds,
                    CorrectSum = rounds.Sum(a => (long)a.AddCoeff),
                }
            };

            dbContext.AddRange(game);
            await dbContext.SaveChangesAsync();
            return game;
        }

        private static List<ConcurrencyGameRound> GenerateRounds(int playerId)
        {
            return Enumerable.Range(0, 5).Select(i => new ConcurrencyGameRound
            {
                AddCoeff = random.Next(100000, 1000000),
                UserId = playerId,
                OrderIndex = i + 1,
            }).ToList();
        }
    }
}
