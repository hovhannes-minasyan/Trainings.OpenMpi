using Microsoft.AspNetCore.SignalR.Client;
using System.Text;

namespace Trainings.OpenMpi.TestApp
{
    public class HubTest
    {
        private HubConnection connection;

        public HubTest()
        {
            var headerValue = "admin:admin";
            var bytes = Encoding.UTF8.GetBytes(headerValue);
            var header = Convert.ToBase64String(bytes);

            this.connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:12000/gamehub", opt =>
                {
                    opt.Headers.Add("Authorization", "Basic " + header);
                })
                .WithAutomaticReconnect()
                .Build();
        }

        public async Task ConnectAsync()
        {
            await connection.StartAsync();
            Console.WriteLine("Hub state : {0}", connection.State);
        }


    }
}
