using Architecture;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Project.Infrastructure.Dapper;

namespace Project.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProject(this IServiceCollection services)
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            SystemDateTime.InitUtcNow(() => DateTime.UtcNow);

            services.AddAllTypes<IRepository>(ServiceLifetime.Transient);
            services.AddAllTypes<IApplicationService>(ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddAllTypes<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T)) && !x.IsInterface && !x.IsAbstract)).ToList();
            foreach (var type in types)
            {
                services.Add(new ServiceDescriptor(type, type, lifetime));

                var interfaces = type.GetInterfaces().ToList();
                foreach (var @interface in interfaces)
                {
                    services.Add(new ServiceDescriptor(@interface, sp => sp.GetRequiredService(type), lifetime));
                }
            }
            return services;
        }
    }
}