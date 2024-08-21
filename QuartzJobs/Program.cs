using QuartzJobs.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddQuartzJobs();

builder.Services.AddDefaultServices();

var app = builder.Build();

app.UseServiceDefaults();

app.Run();