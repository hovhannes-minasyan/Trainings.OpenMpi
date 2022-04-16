using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Trainings.OpenMpi.Dal.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTrainingDb(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("TrainingMpiDbContext");

            services.AddDbContext<TrainingMpiDbContext>(options =>
                options.UseNpgsql(connectionString,
                x => x.MigrationsAssembly("Trainings.OpenMpi.Dal"))
                .UseSnakeCaseNamingConvention());

            return services;
        }

    }
}
