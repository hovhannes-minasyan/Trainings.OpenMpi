using System.Collections.Concurrent;

namespace Trainings.OpenMpi.Api.Services
{
    public class ConnectionStorage
    {
        private readonly ConcurrentDictionary<int, string> activePlayers = new();

        public int PlayersCount => activePlayers.Count;

        public void AddPlayer(int id)
        {
            var isSuccess = activePlayers.TryAdd(id, null);
            if (!isSuccess)
                throw new InvalidOperationException();

        }

        public int[] GetIds() => activePlayers.Keys.ToArray();

        public void RemovePlayer(int id)
        {
            activePlayers.TryRemove(id, out var val);
        }

        public void ClearPlayers()
        {
            activePlayers.Clear();
        }
    }
}
