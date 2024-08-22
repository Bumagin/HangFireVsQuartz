using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.AspNetCore;

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
            
            // Автоматиеческое прерывание задач, которые привысели время выполнения
            cfg.UseJobAutoInterrupt(options =>
            {
                options.DefaultMaxRunTime = TimeSpan.FromMinutes(5);
            });
                
            cfg.AddJobListener<PaymentJobListener>(); // Добавляем подписчика на задачу
        });

        // Добавляем сервер Quartz
        services.AddQuartzServer(o =>
        {
            o.WaitForJobsToComplete = true; // Ждём завершения всех задач перед завершением работы сервера
        });

        ConfigureJobs(services);
    }

    /// <summary>
    /// Конфигурирует Setup классы заданий через рефлексию (Библиотека Scrutor)
    /// </summary>
    private static void ConfigureJobs(IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => 
                classes.AssignableTo<IConfigureOptions<IServiceCollectionQuartzConfigurator>>())
            .AsImplementedInterfaces()
            .WithTransientLifetime());
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