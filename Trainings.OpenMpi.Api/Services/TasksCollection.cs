using System.Collections.Concurrent;

namespace Trainings.OpenMpi.Api.Services
{
    public class TasksCollection
    {
        private readonly BlockingCollection<Task> _tasks;

        public TasksCollection() => _tasks = new BlockingCollection<Task>();

        public void Enqueue(Task settings) => _tasks.Add(settings);

        public Task Dequeue(CancellationToken token) => _tasks.Take(token);
    }
}
