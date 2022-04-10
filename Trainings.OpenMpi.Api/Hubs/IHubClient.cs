using Trainings.OpenMpi.Common;

namespace Trainings.OpenMpi.Api.Hubs
{
    public interface IHubClient
    {
        Task SetConcurrencyGameState(long data);

        Task GameStarted(GameStartedMessage gameStartedMessage);

        Task ConcurrencyGameValueReceived(long value);
    }
}
