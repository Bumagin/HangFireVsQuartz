﻿using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;
using QuartzJobs.Jobs;
using QuartzJobs.Jobs.PaymentJob;

namespace QuartzJobs.DependencyInjection;

public static class ConfigureServices
{
    /// <summary>
    /// Добавляет все зависимости Quartz в IoC контейнер.
    /// </summary>
    public static void AddQuartzJobs(this IServiceCollection services)
    {
        // Добавляем конфигурацию Quartz
        services.AddQuartz(cfg =>
        {
            // Настраиваем использование персистентного хранилища
            cfg.UsePersistentStore(options =>
            {
                // Используем Postgres как хранилище данных для Quartz
                options.UsePostgres(provider =>
                {
                    provider.ConnectionStringName = "Default";
                    provider.TablePrefix = "quartz.qrtz_"; // Префикс для таблиц в базе данных (после точки, перед название схемы)
                });

                options.UseProperties = true; // Рекомендованная настройка маппинга
                options.UseNewtonsoftJsonSerializer(); // Используем Newtonsoft.Json для сериализации данных
            });
            
            cfg.AddJobListener<PaymentJobListener>(); // Добавляем подписчика на задачу

            cfg.ScheduleJob<GarbageJob>(trigger => trigger
                .WithIdentity("Combined Configuration Trigger")
                //.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
                .StartNow()
                .WithDailyTimeIntervalSchedule(interval: 1, intervalUnit: IntervalUnit.Minute)
                .WithDescription("my awesome trigger configured for a job with single call"));
        });

        // Добавляем сервер Quartz
        services.AddQuartzServer(o =>
        {
            o.WaitForJobsToComplete = true; // Ждём завершения всех задач перед завершением работы сервера
        });

        ConfigureJobs(services);
    }

    /// <summary>
    /// Конфигурирует задания (Jobs) для Quartz.
    /// </summary>
    private static void ConfigureJobs(IServiceCollection services)
    {
        services.ConfigureOptions<GarbageJobSetup>(); // Настраиваем задание для очистки мусора
    }

    #region дефолтные

    public static void AddDefaultServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quartz Net API", Version = "v1" });
        });
    }

    public static void UseServiceDefaults(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.MapControllers();

        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
    }

    #endregion
}