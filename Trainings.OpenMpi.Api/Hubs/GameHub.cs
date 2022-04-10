using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Trainings.OpenMpi.Api.Services;

namespace Trainings.OpenMpi.Api.Hubs
{
    [Authorize]
    [Route("gamehub")]
    public class GameHub : Hub<IHubClient>
    {
        private readonly ConnectionStorage storage;
        private readonly ConcurrencyGameService concurrencyGameService;

        protected int UserId => int.Parse(Context.GetHttpContext().User.FindFirst(ClaimTypes.NameIdentifier).Value);

        public GameHub(ConnectionStorage storage, ConcurrencyGameService concurrencyGameService)
        {
            this.storage = storage;
            this.concurrencyGameService = concurrencyGameService;
        }

        public override Task OnConnectedAsync()
        {
            storage.AddPlayer(UserId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            storage.RemovePlayer(UserId);
            return base.OnDisconnectedAsync(exception);
        }


        public async Task SetConcurrencyGameValue(long value)
        {
            var outcome = await concurrencyGameService.SetValueGetNextAsync(UserId, value);
            if (outcome.HasValue)
                await Clients.All.SetConcurrencyGameState(outcome.Value);
        }
    }
}
