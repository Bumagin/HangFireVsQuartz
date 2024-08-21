using Microsoft.Extensions.Options;
using Quartz;

namespace QuartzJobs.Jobs;

public class GarbageJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(GarbageJob));

        options.AddJob<GarbageJob>(j => j.WithIdentity(jobKey));

        /*
         Пример с использованием Cron

         cfg.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithCronSchedule(configuration.GetSection("GarbageJob:CronSchedule").Value));
        */

        // Пример с использованием обычных величин
        
        options.AddTrigger(opts => 
            opts.ForJob(jobKey)
                .WithIdentity(Guid.NewGuid().ToString())
            .WithSimpleSchedule(s =>
                s.WithIntervalInMinutes(1)
                    .RepeatForever()));
    }
}