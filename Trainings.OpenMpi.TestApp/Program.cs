// See https://aka.ms/new-console-template for more information
using Trainings.OpenMpi.TestApp;
using Trainings.OpenMpi.TestApp.GameTests;

Console.WriteLine("Hello, World!");


while (true)
{
    Console.WriteLine("Choose Operation");
    Console.WriteLine("1. Concurrency Game");
    Console.WriteLine("2. Pipeline game");
    var data = Console.ReadLine();
    var isSuccess = int.TryParse(data ?? "0", out var result);

    if (!isSuccess && data.ToLower() == "exit")
        break;

    if (result == 0)
        break;

    if (result == 1)
    {
        var concurrencyGameTest = new ConcurrencyGameTest();
        concurrencyGameTest.StartAsync(2).Wait();
        Console.ReadLine();
    }
    else if (result == 2)
    {
        var pipelineGameTest = new PipelineGameTest();
        pipelineGameTest.StartAsync(2).Wait();
    }
}

Console.WriteLine("Finished");
Console.ReadLine();