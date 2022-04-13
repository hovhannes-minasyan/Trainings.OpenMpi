using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings.OpenMpi.TestApp.GameTests
{
    public class PipelineGameTest
    {
        public async Task StartAsync(int clientCount = 5)
        {
            var collection = new BlockingCollection<int>();

            var playerData = new List<PlayerPipelineGame>(clientCount);
            for (int i = 0; i < clientCount; i++)
            {
                playerData.Add(new PlayerPipelineGame(i, collection));
            }

            foreach (var data in playerData)
                await data.ConnectAsync();

            for (int i = 0; i < clientCount; i++)
            {
                collection.Take();
            }

            foreach (var data in playerData)
                await data.DisconnectAsync();

            Console.WriteLine();
        }
    }
}
