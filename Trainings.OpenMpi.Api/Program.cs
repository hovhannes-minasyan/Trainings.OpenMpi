using inOne.LoyaltySystem.Web.Api.StartupFilters;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.Text.Json.Serialization;
using Trainings.OpenMpi.Api.Authentication;
using Trainings.OpenMpi.Api.Hubs;
using Trainings.OpenMpi.Api.Services;
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

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddTransient<IStartupFilter, TempStartupFilter>();
builder.Services.AddTrainingDb();
builder.Services.AddSingleton<ConnectionStorage>();

builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<NewGameService>();
builder.Services.AddScoped<PipelineGameService>();
builder.Services.AddScoped<ConcurrencyGameService>();

builder.Services.AddSingleton<TasksCollection>();
builder.Services.AddHostedService<BackgroundWorker>();

builder.Services.AddAuthentication(opt =>
{
    var scheme = AuthenticationSchemes.Basic.ToString();
    opt.DefaultScheme = scheme;
    opt.DefaultChallengeScheme = scheme;
    opt.DefaultAuthenticateScheme = scheme;
})
.AddScheme<AuthenticationSchemeOptions, ClientBasicAuthenticationHandler>(AuthenticationSchemes.Basic.ToString(), _ => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<GameHub>("/gamehub");

app.Run();
