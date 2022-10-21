using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Project.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
if (!builder.Environment.IsLocal())
{
    Console.WriteLine("Environment is not `Local`.");
    var app = builder.Build();
}
else
{
    builder.Services.AddDbContext<ProjectDbContext>(opt =>
    {
        var connectionString = builder.Configuration.GetValue<string>("MySqlConnectionString");
        opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b =>
        {
            b.MigrationsAssembly("Project.Migrations");
            b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
        });
    });

    var app = builder.Build();
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    await dbContext.Database.MigrateAsync();
    Console.WriteLine("Done migrations.");
}