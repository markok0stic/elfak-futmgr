using Scheduler.Scheduler;
using Scheduler.Scheduler.Services;
using Shared.Models.DtoModels;
using Shared.Models.MatchModels;
using Shared.Neo4j;
using Shared.Neo4j.DbService;
using Shared.RestApiClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => 
            policyBuilder
                .WithOrigins("https://localhost:7042")
                .AllowAnyHeader()
                .AllowAnyMethod());
});
builder.Services
    .AddNeo4J(builder.Configuration)
    .AddNeo4JDbService<MatchDto, SquadDto>()
    .AddSchedulerOptions(builder.Configuration)
    .AddSingleton<ISchedulerService, SchedulerService>()
    .AddRestApiClient();

var app = builder.Build();
app
    .UseHttpsRedirection()
    .UseRouting()
    .UseCors("AllowSpecificOrigin")
    .UseEndpoints(endpoints => 
    { 
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Schedule}/{action=Index}");
    });
app.Run();