using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Project.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            IConfiguration configuration = configurationBuilder.Build();

            // 這邊需要客製化調整， appsettings.json 的陣列會有資料覆蓋的狀況
            // 所以在設定 appsettings.json 時要特別注意 Serilog.WriteTo 當中的陣列內容順序
            // 假設 appsettings.json 的 Serilog:WriteTo:1 為 LineNotify
            // 那麼其於覆蓋的 appsettings.{ENVIRONMENT}.json 的 Serilog:WriteTo:1 也須為 LineNotify
            // 否則會有資料錯亂的狀況
            var minutesForBlockDuplicatedLog = configuration
                .GetSection("Serilog")
                .GetSection("WriteTo")
                .AsEnumerable()
                .Where(c => c.Key.Contains("MinutesForBlockDuplicatedLog"))
                .Select(c => c.Value)
                .FirstOrDefault();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", AppDomain.CurrentDomain.FriendlyName)
                .Enrich.WithProperty("EnvironmentName", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .Enrich.WithProperty("RuntimeId", Guid.NewGuid().ToString())
                .Enrich.WithProperty("MinutesForBlockDuplicatedLog", minutesForBlockDuplicatedLog)
                .CreateLogger();

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", AppDomain.CurrentDomain.FriendlyName);
                IHost host = CreateHostBuilder(args).Build();

                Log.Information("Starting web host ({ApplicationContext})...", AppDomain.CurrentDomain.FriendlyName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppDomain.CurrentDomain.FriendlyName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();
    }
}
