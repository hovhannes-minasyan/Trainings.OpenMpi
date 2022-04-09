using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trainings.OpenMpi.Api.Models;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace Trainings.OpenMpi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {      
        private readonly ILogger<UsersController> _logger;
        private readonly TrainingMpiDbContext context;

        public UsersController(ILogger<UsersController> logger, TrainingMpiDbContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task<ActionResult<User>> AddUserAsync(UserCreateModel model) 
        {
            var user = context.Users.Add(new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
            }).Entity;
            await context.SaveChangesAsync();

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await context.Users.ToArrayAsync();
        }
    }
}