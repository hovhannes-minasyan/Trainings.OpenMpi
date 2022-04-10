using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using Trainings.OpenMpi.Common;

namespace Trainings.OpenMpi.TestApp
{
    public class PlayerConcurrencyGame
    {
        private long number;
        private HubConnection connection;

        public PlayerConcurrencyGame(int i)
        {
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
            connection.On<GameStartedMessage>("GameStarted", GameStarted);
            connection.On<long>("ConcurrencyGameValueReceived", ConcurrencyGameValueReceived);
        }

        public Task ConnectAsync()
        {
            return connection.StartAsync();
        }

        private void SetConcurrencyGameState(long number)
        {
            this.number = number;
        }

        private void GameStarted(GameStartedMessage gameStartedMessage)
        {

        }

        private async Task ConcurrencyGameValueReceived(long value)
        {
            await Task.Delay(3000);
            await connection.SendAsync("SetConcurrencyGameValue", number + value);
        }
    }
}
