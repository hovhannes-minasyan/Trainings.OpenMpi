using Trainings.OpenMpi.Common;

namespace Trainings.OpenMpi.Api.Hubs
{
    public interface IHubClient
    {
        Task SetConcurrencyGameState(long data);

        Task GameStarted(GameEventMessage gameStartedMessage);

        Task ConcurrencyGameValueReceived(long value);

        Task GameEnded();


        Task ResubmitPipelineStep();
        Task ReceivePipelineStep(PipelineStepMessage pipelineStepMessage);
    }
}
