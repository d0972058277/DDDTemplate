using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Project.Infrastructure;

namespace Project.Migrations
{
    public class ProjectDbContextFactory : IDesignTimeDbContextFactory<ProjectDbContext>
    {
        public ProjectDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=127.0.0.1; Port=3306; User ID=root; Password=root; Database=Project;";
            var optionsBuilder = new DbContextOptionsBuilder<ProjectDbContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b =>
            {
                b.MigrationsAssembly("Project.Migrations");
                b.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            });
            return new ProjectDbContext(optionsBuilder.Options);
        }
    }
}