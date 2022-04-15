using Microsoft.AspNetCore.Mvc;
using Trainings.OpenMpi.Api.Services;
using Trainings.OpenMpi.Common.Enums;
using Trainings.OpenMpi.Dal;

namespace Trainings.OpenMpi.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly TrainingMpiDbContext dbContext;
        private readonly GameService gameService;

        public GamesController(TrainingMpiDbContext dbContext, GameService gameService)
        {
            this.dbContext = dbContext;
            this.gameService = gameService;
        }

        [HttpPost]
        public async Task<ActionResult> StartGameAsync(GameType gameType)
        {
            await gameService.StartGameAsync(gameType);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            var result = await gameService.GetAllAsync();
            return Ok(result);
        }
    }
}
