using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Project.Infrastructure;

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
    });

    // NOTE: 用來顯示 Sql 參數化的參數內容
    // builder.EnableSensitiveDataLogging();
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(p => !p.IsDynamic), ServiceLifetime.Transient);

builder.Services.AddProject();

var app = builder.Build();

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
