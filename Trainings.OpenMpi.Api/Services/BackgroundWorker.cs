namespace Trainings.OpenMpi.Api.Services
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly TasksCollection _tasks;

        private CancellationTokenSource _tokenSource;

        public BackgroundWorker(TasksCollection tasks) => _tasks = tasks;

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(1, cancellationToken); // Use this to actually start the background task
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            while (cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    var taskToRun = _tasks.Dequeue(_tokenSource.Token);

                    // We need to save executable task, 
                    // so we can gratefully wait for it's completion in Stop method
                    await taskToRun;
                }
                catch (OperationCanceledException)
                {
                    // execution cancelled
                }
            }
        }
    }
}
