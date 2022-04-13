using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Concurrent;
using System.Text;
using Trainings.OpenMpi.Common;

namespace Trainings.OpenMpi.TestApp.GameTests
{
    public class PlayerConcurrencyGame
    {
        private int id;
        private readonly BlockingCollection<int> collection;
        private long number;
        private HubConnection connection;

        public PlayerConcurrencyGame(int i, BlockingCollection<int> collection)
        {
            id = i;
            this.collection = collection;
            var headerValue = $"player{i}:player{i}";
            var bytes = Encoding.UTF8.GetBytes(headerValue);
            var header = Convert.ToBase64String(bytes);

            this.connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:12000/gamehub", opt =>
                {
                    opt.Headers.Add("Authorization", "Basic " + header);
                })
                .WithAutomaticReconnect()
                .Build();


            connection.On<long>("SetConcurrencyGameState", SetConcurrencyGameState);
            connection.On<GameEventMessage>("GameStarted", GameStarted);
            connection.On<long>("ConcurrencyGameValueReceived", ConcurrencyGameValueReceived);
            connection.On("GameEnded", GameEnded);
        }

        public Task DisconnectAsync()
        {
            return connection.StopAsync();
        }

        public Task ConnectAsync()
        {
            return connection.StartAsync();
        }

        private void SetConcurrencyGameState(long number)
        {
            Console.WriteLine($"{id}: Number changed to {number}");
            this.number = number;
        }

        private void GameStarted(GameEventMessage gameStartedMessage)
        {
            Console.WriteLine($"{id}: Game Started");
        }

        private void GameEnded() 
        {
            Console.WriteLine("Game Ended");
            collection.Add(id);
        }

        private async Task ConcurrencyGameValueReceived(long value)
        {
            Console.WriteLine($"{id}: Sending Starts");
            await Task.Delay(3000);
            
            Console.WriteLine($"{id}:Sending {number + value}");
            await connection.SendAsync("SetConcurrencyGameValue", number + value);
        }
    }
}
