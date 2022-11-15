using Quartz;
using appointmentLookupApi;
using appointmentLookupModel;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddJsonFile("appsettings."+Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")+".json", optional: true).AddEnvironmentVariables().Build();
//Add dependency injections
builder.Services.AddTransient<IStatusService, StatusService>();

builder.Services.AddScoped<ICGEService, CGEService>();
builder.Services.AddScoped<ILookupResultsDbHandler, LookupResultDbHandler>();
builder.Services.AddScoped<ICGEWebHandler, CGEWebHandler>();

builder.Services.AddSingleton<IVersionInfo, VersionInfo>();
builder.Services.AddSingleton<ITelegramWebHandler, TelegramWebHandler>();
builder.Services.AddSingleton<ICGEStateManager, CGEStateManager>();

builder.Services.AddAutoMapper(configuration =>
    { 
        configuration.CreateMap<LookupResult, DTOLookupResult>().ReverseMap();
        configuration.CreateMap<LookupResult, DTOLookupResultWithDetails>().ReverseMap();
    }
,typeof(Program)); 
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("CGEJob");
    q.AddJob<CGEJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CGEJob-trigger")
        .WithCronSchedule(config.GetSection("Quartz:CronSchedule").Value));

});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("appointmentLookupDatabase"));
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
