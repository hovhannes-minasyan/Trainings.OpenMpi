using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Common;
using Trainings.OpenMpi.Common.Enums;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Services
{
    public class GameService
    {
        private readonly ConcurrencyGameService concurrencyGameService;
        private readonly IHubContext<GameHub, IHubClient> hubContext;
        private readonly TrainingMpiDbContext dbContext;

        public GameService(ConcurrencyGameService concurrencyGameService, IHubContext<GameHub, IHubClient> hubContext, TrainingMpiDbContext dbContext)
        {
            this.concurrencyGameService = concurrencyGameService;
            this.hubContext = hubContext;
            this.dbContext = dbContext;
        }

        public async Task StartGameAsync(GameType gameType)
        {
            var game = gameType switch
            {
                GameType.Concurrency => await concurrencyGameService.StartGameAsync(),
                _ => throw new ArgumentException(),
            };

            await hubContext.Clients.All.GameStarted(new GameStartedMessage()
            {
                GameId = game.Id,
                GameType = gameType,
            });
        }

        public async Task SendStartupDataAsync(Game game)
        {
            if (game.Type == GameType.Concurrency)
            {
                var rounds = await dbContext.ConcurrencyGameRounds.ToArrayAsync();
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
}
