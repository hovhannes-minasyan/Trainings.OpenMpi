using Microsoft.EntityFrameworkCore;
using Npgsql;
using Trainings.OpenMpi.Dal;
using Trainings.OpenMpi.Dal.Entities;

namespace inOne.LoyaltySystem.Web.Api.StartupFilters
{
    public class TempStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                using var context = builder.ApplicationServices.CreateScope().ServiceProvider.GetService<TrainingMpiDbContext>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                using var connection = context.Database.GetDbConnection() as NpgsqlConnection;
                connection.Open();
                connection.ReloadTypes();

                next(builder);

                var user = new User
                {
                    FirstName = "Hovhannes",
                    LastName = "Minasyan",
                };
                context.Add(user);
                context.SaveChanges();
            };
        }

    }
}
