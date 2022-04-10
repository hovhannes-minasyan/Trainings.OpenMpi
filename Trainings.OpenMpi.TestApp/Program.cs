// See https://aka.ms/new-console-template for more information
using Trainings.OpenMpi.TestApp;

Console.WriteLine("Hello, World!");


while (true)
{
    Console.WriteLine("Choose Operation");
    Console.WriteLine("1. Connect to hub");
    Console.WriteLine("2. Post a game");
    var result = int.Parse(Console.ReadLine() ?? "0");
    if (result == 0)
        break;

    if (result == 1)
    {
        var hubTest = new HubTest();
        hubTest.ConnectAsync().Wait();
    }
    else if (result == 2)
    {
        var controllerTest = new ControllerTest();
        controllerTest.CreateGameAsync().Wait();
    }
}

Console.WriteLine("Finished");
Console.ReadLine();