using Microsoft.AspNetCore.SignalR;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Api.Services;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Abstractions
{
    public abstract class BaseGameService
    {
        protected static readonly Random random = new Random();
        protected readonly TrainingMpiDbContext dbContext;
        protected readonly ConnectionStorage connectionStorage;
        protected readonly IHubContext<GameHub, IHubClient> hubContext;

        public BaseGameService(TrainingMpiDbContext dbContext, ConnectionStorage connectionStorage, IHubContext<GameHub, IHubClient> hubContext)
        {
            this.dbContext = dbContext;
            this.connectionStorage = connectionStorage;
            this.hubContext = hubContext;
        }

        public abstract Task<Game> CreateGameAsync();

        public abstract Task StartGameAsync(Game game);
    }
}
