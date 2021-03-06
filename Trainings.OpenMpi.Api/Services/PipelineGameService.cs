using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Api.Abstractions;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Common;
using Trainings.OpenMpi.Common.Enums;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Services
{
    public class PipelineGameService : BaseGameService
    {
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);
        public PipelineGameService(
            TrainingMpiDbContext dbContext, 
            ConnectionStorage connectionStorage, 
            IHubContext<GameHub, IHubClient> hubContext) : 
            base(dbContext, connectionStorage, hubContext)
        {
        }

        public override async Task<Game> CreateGameAsync() 
        {
            var playerCount = connectionStorage.PlayersCount;

            var game = new Game()
            {
                IsActive = true,
                Type = GameType.Pipeline,
                PipelineGame = new PipelineGame()
                {
                    RoundsLeft = 5,
                    PipelineLength = playerCount,
                    PipelineSteps = await CreateSteps(),
                }
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return game;
        }

        public override async Task StartGameAsync(Game game)
        {
            connectionStorage.RedefineNeighbors();

            var firstPlayer = connectionStorage.GetIds().Min();
            var step = await dbContext.PipelineSteps.FirstOrDefaultAsync(s => s.UserId == firstPlayer && s.PipelineGameId == game.Id);            
            await SendTaskTo(step, random.Next(1000,10000));
        }

        private async Task<PipelineStep[]> CreateSteps() 
        {
            var questions = await dbContext.QuizQuestions.ToArrayAsync();
            
            return connectionStorage
                .GetIds()
                .Select(id=>new PipelineStep 
                {
                    Operation = SimpleOperationType.Addition,
                    QuizQuestionId = questions[random.Next(questions.Length)].Id,
                    UserId = id,
                    State = PipelineState.Free,
                    DataValue = 0,
                })
                .ToArray();
        }

        public async Task CompleteStepAsync(int playerId, decimal data) 
        {
            await semaphore.WaitAsync();
            try 
            {
                var game = await dbContext.PipelineGames
                    .Include(a => a.PipelineSteps)
                    .ThenInclude(a => a.QuizQuestion)
                    .FirstOrDefaultAsync(a => a.Game.IsActive);

                var nextPlayer = connectionStorage.GetNextNeighborIfExists(playerId);
                var prevPlayer = connectionStorage.GetPrevNeighborIfExists(playerId);
                var myStep = game.PipelineSteps.First(a => a.UserId == playerId);

                myStep.DataValue = data;

                if (nextPlayer.HasValue)
                {
                    var nextStep = game.PipelineSteps.First(a => a.UserId == nextPlayer);
                    if (nextStep.State != PipelineState.Free)
                    {
                        await dbContext.SaveChangesAsync();
                        myStep.State = PipelineState.WaitingToSend;
                        return;
                    }
                    await SendTaskTo(nextStep, data);
                }

                myStep.State = PipelineState.Free;
                await dbContext.SaveChangesAsync();

                if(nextPlayer == null && game.PipelineSteps.Count(a => a.State != PipelineState.Free) == 0) 
                {
                    if(prevPlayer != null || game.RoundsLeft == 0)
                        await hubContext.Clients.All.GameEnded();
                }

                if (prevPlayer == null)
                {
                    if (game.RoundsLeft == 0)
                        return;

                    game.RoundsLeft--;
                    await dbContext.SaveChangesAsync();

                    await SendTaskTo(myStep, random.Next(1000, 10000));
                    myStep.State = PipelineState.Working;

                    await dbContext.SaveChangesAsync();
                }
                else
                {
                    var prevStep = game.PipelineSteps.First(a => a.UserId == prevPlayer);
                    if (prevStep.State != PipelineState.WaitingToSend)
                        return;

                    await hubContext.Clients.User(prevPlayer.ToString()).ResubmitPipelineStep();
                }
            }
            finally 
            {
                semaphore.Release();
            }            
        }

        private Task SendTaskTo(PipelineStep step, decimal data) 
        {
            return hubContext.Clients
                .User(step.UserId.ToString())
                .ReceivePipelineStep(new PipelineStepMessage 
                {
                    Data = data,
                    Operation = step.Operation.ToString(),
                    Question = step.QuizQuestion.Question,
                });
        }
    }
}
