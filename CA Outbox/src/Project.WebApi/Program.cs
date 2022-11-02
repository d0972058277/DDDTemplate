using CorrelationId;
using CorrelationId.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Project.Infrastructure;
using Project.Infrastructure.Masstransit.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/2394
    options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
});
builder.Services.AddSwaggerGenNewtonsoftSupport();

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

    x.UsingInMemory((context, config) =>
    {
        config.ConfigureEndpoints(context);
        config.MessageTopology.FormattEntityName(environmentNamePrefix);
    });
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic), ServiceLifetime.Transient);
builder.Services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDefaultCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.RequestHeader = CorrelationIdOptions.DefaultHeader.ToLower();
});

builder.Services.AddProject();

var app = builder.Build();

app.UseCorrelationId();

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
