using Microsoft.Extensions.Options;
using Quartz;

namespace QuartzJobs.Jobs;

public class GarbageJobSetup : IConfigureOptions<IServiceCollectionQuartzConfigurator>
{
    public void Configure(IServiceCollectionQuartzConfigurator options)
    {
        options.ScheduleJob<GarbageJob>(trigger => trigger
            .WithIdentity("Combined Configuration Trigger")
            //.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
            .StartNow()
            .WithDailyTimeIntervalSchedule(interval: 1, intervalUnit: IntervalUnit.Minute)
            .WithDescription("my awesome trigger configured for a job with single call"));
    }
}