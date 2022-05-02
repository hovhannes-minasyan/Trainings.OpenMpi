using Microsoft.AspNetCore.SignalR;
using Trainings.OpenMpi.Api.Abstractions;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Services
{
    public class NewGameService : BaseGameService
    {
        public NewGameService(TrainingMpiDbContext dbContext, ConnectionStorage connectionStorage, IHubContext<GameHub, IHubClient> hubContext) : base(dbContext, connectionStorage, hubContext)
        {
        }

        public override Task<Game> CreateGameAsync()
        {
            throw new NotImplementedException();
        }

        public override Task StartGameAsync(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
