using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Project.Infrastructure;

namespace Project.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Serilog>(Configuration.GetSection(nameof(Serilog)));

            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                var info = new OpenApiInfo { Title = $"{AppDomain.CurrentDomain.FriendlyName}", Version = "v1" };

                c.SwaggerDoc("v1", info);

                c.CustomSchemaIds(type => type.FullName);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddProject();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project.WebApi v1");
            });
        }
    }
}
