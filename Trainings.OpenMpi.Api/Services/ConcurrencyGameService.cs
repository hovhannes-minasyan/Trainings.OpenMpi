using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Api.Abstractions;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Common;
using Trainings.OpenMpi.Common.Enums;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Services
{
    public class ConcurrencyGameService : BaseGameService
    {
        private static SemaphoreSlim gameFinishedLock = new(1);

        public ConcurrencyGameService(
            ConnectionStorage connectionStorage, 
            TrainingMpiDbContext dbContext, 
            IHubContext<GameHub, IHubClient> hubContext) : 
            base(dbContext, connectionStorage, hubContext)
        {
        }

        public async Task SetValueGetNextAsync(int userId, long number)
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
            var gameRoundsData = await gameQuery.Take(2).ToArrayAsync();

            if (!gameRoundsData.Any())
                throw new ArgumentException();

            var game = gameRoundsData.First().Game;
            var finishedRound = gameRoundsData.First().Round;

            await Task.Delay(random.Next(1000, 3000));

            game.CurrentSum = number;
            finishedRound.IsFinished = true;
            dbContext.Update(game);
            dbContext.Update(finishedRound);
            await dbContext.SaveChangesAsync();

            if (gameRoundsData.Length == 1) 
            {
                await CheckIfGameEnded(game.GameId);
                return;
            }

            await hubContext.Clients.All.SetConcurrencyGameState(gameRoundsData[1].Round.AddCoeff);

            var nextRound = gameRoundsData.Last().Round;
            await hubContext.Clients.User(userId.ToString()).ConcurrencyGameValueReceived(nextRound.AddCoeff);
        }

        public override async Task<Game> CreateGameAsync()
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

        private async Task CheckIfGameEnded(int gameId)
        {
            await gameFinishedLock.WaitAsync();
            try
            {
                var count = await dbContext.ConcurrencyGameRounds.CountAsync(a => a.ConcurrencyGameId == gameId && !a.IsFinished);

                if (count > 0)
                    return;

                var game = await dbContext.Games.FindAsync(gameId);
                if (!game.IsActive)
                    return;

                game.IsActive = false;
                dbContext.Update(game);
                await dbContext.SaveChangesAsync();
            }
            finally
            {
                gameFinishedLock.Release();
            }

            await hubContext.Clients.All.GameEnded();

            Console.WriteLine("Concurrency Game Ended Message Sent");
        }

        public override async Task StartGameAsync(Game game) 
        {
            var rounds = await dbContext.ConcurrencyGameRounds.Where(r => r.ConcurrencyGameId == game.Id).ToArrayAsync();
            var data = rounds.GroupBy(r => r.UserId).Select(gr => new
            {
                Value = gr.Where(e => !e.IsFinished).OrderBy(e => e.OrderIndex).First().AddCoeff,
                UserId = gr.Key
            }).ToArray();

            foreach (var row in data)
            {
                await hubContext.Clients.User(row.UserId.ToString()).ConcurrencyGameValueReceived(row.Value);
            }
        }
    }
}
