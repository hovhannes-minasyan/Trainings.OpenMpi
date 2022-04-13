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
    public class GameService
    {
        private readonly ConcurrencyGameService concurrencyGameService;
        private readonly IHubContext<GameHub, IHubClient> hubContext;
        private readonly TrainingMpiDbContext dbContext;
        private readonly PipelineGameService pipelineGameService;

        public GameService(
            ConcurrencyGameService concurrencyGameService, 
            IHubContext<GameHub, IHubClient> hubContext, 
            TrainingMpiDbContext dbContext,
            PipelineGameService pipelineGameService)
        {
            this.concurrencyGameService = concurrencyGameService;
            this.hubContext = hubContext;
            this.dbContext = dbContext;
            this.pipelineGameService = pipelineGameService;
        }

        public Task<Game[]> GetAllAsync()
        {
            return dbContext.Games.ToArrayAsync();
        }

        public async Task StartGameAsync(GameType gameType)
        {
            var service = gameType switch
            {
                GameType.Concurrency => concurrencyGameService as BaseGameService,
                GameType.Pipeline => pipelineGameService,
                _ => throw new ArgumentException(),
            };

            var game = await service.CreateGameAsync();

            await hubContext.Clients.All.GameStarted(new GameEventMessage()
            {
                GameId = game.Id,
                GameType = gameType,
            });

            await service.StartGameAsync(game);
        }
    }
}
