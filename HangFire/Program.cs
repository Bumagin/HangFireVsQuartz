using HangFire.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddHangFireJobs(builder.Configuration);

builder.Services.AddDefaultServices();

var app = builder.Build();

app.UseHangFireDashBoard();

app.UseServiceDefaults();

app.Run();