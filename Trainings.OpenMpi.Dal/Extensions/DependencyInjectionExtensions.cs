using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainings.OpenMpi.Dal.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddTrainingDb(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetService<IConfiguration>();

            services.AddDbContext<TrainingMpiDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("TrainingMpiDbContext"),
                x => x.MigrationsAssembly("Trainings.OpenMpi.Dal"))
                .UseSnakeCaseNamingConvention());

            return services;
        }

    }
}
