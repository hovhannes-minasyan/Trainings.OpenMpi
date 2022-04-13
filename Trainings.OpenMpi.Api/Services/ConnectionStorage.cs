using System.Collections.Concurrent;

namespace Trainings.OpenMpi.Api.Services
{
    public class ConnectionStorage
    {
        private readonly List<int> neighbors= new List<int>();
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

        public void RedefineNeighbors() 
        {
            neighbors.Clear();
            neighbors.AddRange(activePlayers.Select(a=>a.Key).OrderBy(i => i).ToArray());
        }

        public int[] GetNeighbors(int playerId) 
        {
            var arr = new int[2];
            var index = neighbors.FindIndex(a=> a == playerId);
            arr[0] = index - 1;
            arr[1] = index + 1;
            
            if(arr[0] < 0) 
                arr[0] = neighbors.Count - 1;
            
            if (arr[1] == 
                neighbors.Count) arr[1] = 0;

            arr[0] = neighbors[arr[0]];
            arr[1] = neighbors[arr[1]];

            return arr;
        }

        // Player Id or -1
        public int? GetNextNeighborIfExists(int playerId)
        {
            var arr = new int[2];
            var index = neighbors.FindIndex(a => a == playerId);

            return index == neighbors.Count - 1 ? null : neighbors[index + 1];
        }

        // Player Id or -1
        public int? GetPrevNeighborIfExists(int playerId)
        {
            var arr = new int[2];
            var index = neighbors.FindIndex(a => a == playerId);
            return index == 0 ? null : neighbors[index - 1];
        }
    }
}
