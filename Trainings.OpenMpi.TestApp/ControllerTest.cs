using System.Net.Http.Headers;
using System.Text;

namespace Trainings.OpenMpi.TestApp
{
    public class ControllerTest
    {
        HttpClient client = new HttpClient();

        public ControllerTest()
        {
            client.BaseAddress = new Uri("http://localhost:12000");

            var headerValue = "admin:admin";
            var bytes = Encoding.UTF8.GetBytes(headerValue);
            var header = Convert.ToBase64String(bytes);
            //client.DefaultRequestHeaders.Add("Authorization", "Basic " + header);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);
        }

        public async Task CreateGameAsync()
        {
            var result = await client.PostAsync("api/games", null);

            Console.WriteLine($"Response status = {result.StatusCode}");
        }
    }
}
