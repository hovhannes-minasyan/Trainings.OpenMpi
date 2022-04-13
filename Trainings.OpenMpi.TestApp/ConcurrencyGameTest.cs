using System.Collections.Concurrent;

namespace Trainings.OpenMpi.TestApp
{
    public class ConcurrencyGameTest
    {
        public async Task StartAsync(int clientCount = 5)
        {
            var collection = new BlockingCollection<int>();

            var playerData = new List<PlayerConcurrencyGame>(clientCount);
            for (int i = 0; i < clientCount; i++)
            {
                playerData.Add(new PlayerConcurrencyGame(i, collection));
            }

            foreach (var data in playerData)
            {
                await data.ConnectAsync();
            }

            for (int i = 0; i < clientCount; i++)
            {
                collection.Take();
            }
        }
    }
}
