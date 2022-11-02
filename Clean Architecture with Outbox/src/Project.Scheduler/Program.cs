using CorrelationId;
using CorrelationId.DependencyInjection;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Infrastructure;
using Project.Infrastructure.Masstransit.Extensions;
using Project.Scheduler;
using Project.Scheduler.JobListeners;
using Project.Scheduler.Jobs;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAllTypes<IGeneralJob>(ServiceLifetime.Transient);

builder.Services.AddQuartz(options =>
{
    options.UseJobFactory<JobFactory>();
    options.AddJobListener<FailedJobListener>();

    options.AddJob<PublishTimeoutIntegrationEventsJob>(opts => opts.WithIdentity(nameof(PublishTimeoutIntegrationEventsJob)));
    options.AddTrigger(opts =>
    {
        opts.ForJob(nameof(PublishTimeoutIntegrationEventsJob))
            .WithIdentity(nameof(PublishTimeoutIntegrationEventsJob) + "Trigger")
            .WithCronSchedule("0/15 * * * * ? *");
    });
});
builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddDbContext<ProjectDbContext>(dbContextOptionsBuilder =>
{
    var connectionString = builder.Configuration.GetValue<string>("MySqlConnectionString");
    dbContextOptionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptionsAction =>
    {
        mySqlOptionsAction.CommandTimeout(5);
        mySqlOptionsAction.EnableRetryOnFailure();
    });

    // NOTE: 用來顯示 Sql 參數化的參數內容
    // builder.EnableSensitiveDataLogging();
});
builder.Services.AddDbContext<ReadonlyProjectDbContext>(dbContextOptionsBuilder =>
{
    // TODO: 當有 Slave 的資料庫時，需要修改這段連線字串
    var connectionString = builder.Configuration.GetValue<string>("MySqlConnectionString");
    dbContextOptionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
    {
        options.CommandTimeout(5);
    }).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution);
});

builder.Services.AddMassTransit(x =>
{
    var environmentNamePrefix = builder.Environment.EnvironmentName;

    x.FormatEndpointName(environmentNamePrefix);

    x.UsingRabbitMq((context, config) =>
    {
        var host = builder.Configuration.GetValue<string>("RabbitMqHost");
        var username = builder.Configuration.GetValue<string>("RabbitMqUsername");
        var password = builder.Configuration.GetValue<string>("RabbitMqPassword");

        config.Host(host, "/", host =>
        {
            host.Username(username);
            host.Password(password);
        });

        config.ConfigureEndpoints(context);

        config.MessageTopology.FormattEntityName(environmentNamePrefix);
    });
});

builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDefaultCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.RequestHeader = CorrelationIdOptions.DefaultHeader.ToLower();
});

builder.Services.AddProject();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetRequiredService<ILogger<Program>>().LogInformation("ApplicationStarted。");
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    //優雅的停止站台
    //雖然Server已經停止，但load balance不一定會即時更新，request會送到已停止的Server
    //這裡使用Sleep，確保 'Server停止' 比 'load balance更新' 更晚
    //詳細，https://blog.markvincze.com/graceful-termination-in-kubernetes-with-asp-net-core/
    app.Services.GetRequiredService<ILogger<Program>>().LogInformation($"ApplicationStopping。");
    Thread.Sleep(60000);
    app.Services.GetRequiredService<ILogger<Program>>().LogInformation($"ApplicationStopped。");
});

app.Run();
