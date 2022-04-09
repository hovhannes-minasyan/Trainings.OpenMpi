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
                next(builder);

                using var scope = builder.ApplicationServices.CreateScope();
                using var context = scope.ServiceProvider.GetService<TrainingMpiDbContext>();
                context.Database.Migrate();

                using var connection = context.Database.GetDbConnection() as NpgsqlConnection;
                connection.Open();
                connection.ReloadTypes();

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
