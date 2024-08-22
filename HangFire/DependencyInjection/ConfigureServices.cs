using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;
using SharedKernel.Jobs;

namespace HangFire.DependencyInjection;

public static class ConfigureServices
{
    /// <summary>
    /// Добавляет все зависимости HangFire в IoC контейнер
    /// </summary>
    public static void AddHangFireJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config =>
        {
            // Установка уровня совместимости данных
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                // Использование простого сериализатора по имени сборки
                .UseSimpleAssemblyNameTypeSerializer()
                // Рекомендуемые настройки сериализации
                .UseRecommendedSerializerSettings()
                // Настройка хранения данных в PostgreSQL
                .UsePostgreSqlStorage(cfg =>
                {
                    // Использование строки подключения из конфигурации
                    cfg.UseNpgsqlConnection(configuration.GetConnectionString("Default"));
                });
        });

        // Добавление сервера Hangfire
        services.AddHangfireServer(cfg =>
        {
        //    cfg.StopTimeout = TimeSpan.FromMinutes(5);
        });
    }

    /// <summary>
    /// Настраивает панель управления Hangfire в приложении и включает авторизацию панели.
    /// </summary>
    public static void UseHangFireDashBoard(this WebApplication app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            // Настройка фильтров авторизации доски (пустой массив означат отсутствие авторизации)
            Authorization = Array.Empty<IDashboardAuthorizationFilter>(),
        });

        ConfigureRecurringJobs(app.Configuration);
    }

    /// <summary>
    /// Конфигурирует повторяющиеся задачи Hangfire, устанавливая периодичность и выполняемый метод.
    /// </summary>
    private static void ConfigureRecurringJobs(IConfiguration configuration)
    {
        RecurringJob.AddOrUpdate<GarbageJob>(
            "recurring-job",
            job => job.DoSomething(),
            configuration.GetSection("GarbageJob:CronSchedule").Value, // Частота выполнения в Cron
            new RecurringJobOptions() // Дополнительные параметры для задачи
        );
    }

    #region дефолтные

    public static void AddDefaultServices(this IServiceCollection services)
    {
        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HangFire API", Version = "v1" });
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