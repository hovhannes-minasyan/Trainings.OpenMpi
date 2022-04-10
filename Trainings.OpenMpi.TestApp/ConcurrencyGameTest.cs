namespace Trainings.OpenMpi.TestApp
{
    public class ConcurrencyGameTest
    {
        public async Task StartAsync()
        {
            var playerData = new List<PlayerConcurrencyGame>();
            for (int i = 0; i < 10; i++)
            {
                playerData.Add(new PlayerConcurrencyGame(i));
            }


            foreach (var data in playerData)
            {
                await data.ConnectAsync();
            }


        }

    }
}
