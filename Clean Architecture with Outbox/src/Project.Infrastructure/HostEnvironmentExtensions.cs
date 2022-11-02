using Microsoft.Extensions.Hosting;

namespace Project.Infrastructure
{
    public static class HostEnvironmentExtensions
    {
        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.EnvironmentName.ToLower() == "local")
                return true;
            else
                return false;
        }
    }
}