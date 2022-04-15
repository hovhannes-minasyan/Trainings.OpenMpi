using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainings.OpenMpi.Common;

namespace Trainings.OpenMpi.TestApp.GameTests
{
    public class PlayerPipelineGame
    {
        private static Random random = new Random();
        private readonly int id;
        private readonly BlockingCollection<int> collection;
        private readonly HubConnection connection;

        private decimal currentData = 0;

        public PlayerPipelineGame(int i, BlockingCollection<int> collection)
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


            connection.On("ResubmitPipelineStep", ResubmitPipelineStep);
            connection.On<GameEventMessage>("GameStarted", GameStarted);
            connection.On<PipelineStepMessage>("ReceivePipelineStep", ReceivePipelineStep);
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

        private void GameStarted(GameEventMessage gameEventMessage) 
        {
            Console.WriteLine("Game started");
        }

        private void GameEnded()
        {
            Console.WriteLine("Game Ended");
            collection.Add(id);
        }

        private async Task ResubmitPipelineStep() 
        {
            Console.WriteLine("Resubmit Request");
            await connection.SendAsync("CompletePipelineGameStep", currentData);
        }

        private async Task ReceivePipelineStep(PipelineStepMessage pipelineStepMessage) 
        {
            Console.WriteLine("Step Received");
            currentData += pipelineStepMessage.Data;

            await Task.Delay(random.Next(1000, 5000));
            Console.WriteLine($"Sending message CompletePipelineGameStep - {currentData}");
            await connection.SendAsync("CompletePipelineGameStep", currentData);
        }
    }
}
