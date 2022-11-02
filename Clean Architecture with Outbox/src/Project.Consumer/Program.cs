using CorrelationId;
using CorrelationId.DependencyInjection;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Application;
using Project.Infrastructure;
using Project.Infrastructure.Masstransit.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

    x.AddApplicationConsumers<ApplicationAssemblyHelper>();

    x.AddConfigureEndpointsCallback((queueName, receiveEndpointConfigurator) =>
    {
        receiveEndpointConfigurator.RethrowFaultedMessages();

        // NOTE: https://masstransit-v6.netlify.app/usage/exceptions.html#retry-configuration
        // 伺服器端的重試，貌似會有使用較多記憶體的狀況，且不會釋放I/O資源，故先註解觀察
        // receiveEndpointConfigurator.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)));

        // NOTE: https://masstransit-project.com/advanced/middleware/concurrency-limit.html
        // 通過指定併發消息限制，MassTransit 可以限制同時傳遞給消費者的消息數。同時，由於消費者工廠用於創建消費者，因此它還限制了同時存在的併發使用者的數量。
        // 提示: 併發消息限制適用於消費者使用的所有消息類型的總數。
        // receiveEndpointConfigurator.UseConcurrencyLimit(Environment.ProcessorCount * 2);
    });

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

        // config.UseConsumeFilter(typeof(CorrelationBehavior<>), context);
        // https://github.com/MassTransit/MassTransit/blob/3b6a9114bc4e776eb1c0e95210dd3d49064f2958/src/MassTransit/Configuration/DependencyInjection/DependencyInjectionFilterExtensions.cs#L22
        // config.UseConsumerConsumeFilter(typeof(LoggingBehavior<,>), context);
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
