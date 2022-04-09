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

            var connectionString = configuration.GetConnectionString("TrainingMpiDbContext");
            
            Console.WriteLine($"CONNECTION STRING = {connectionString}");
            
            services.AddDbContext<TrainingMpiDbContext>(options =>
                options.UseNpgsql(connectionString,
                x => x.MigrationsAssembly("Trainings.OpenMpi.Dal"))
                .UseSnakeCaseNamingConvention());

            return services;
        }

    }
}
