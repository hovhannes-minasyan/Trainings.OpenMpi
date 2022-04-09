using inOne.LoyaltySystem.Web.Api.StartupFilters;
using Trainings.OpenMpi.Dal.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureHostConfiguration(hostConfig =>
{
    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
    hostConfig.AddJsonFile("appsettings.json", optional: false);
    hostConfig.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
    hostConfig.AddEnvironmentVariables(prefix: "ASPNETCORE_");
    hostConfig.AddCommandLine(args);
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTrainingDb();
builder.Services.AddTransient<IStartupFilter, TempStartupFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
