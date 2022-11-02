using MassTransit;

namespace Project.Infrastructure.Masstransit.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IBusRegistrationConfigurator AddApplicationConsumers<T>(this IBusRegistrationConfigurator configure)
        {
            var justLoadingApplication = typeof(T);
            var rootDomainName = typeof(ServiceCollectionExtensions).Namespace!.Split('.').First();
            var consumerAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName!.Contains(rootDomainName))
                .SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(IConsumer)) && !x.IsInterface && !x.IsAbstract))
                .Select(t => t.Assembly).ToArray();
            configure.AddConsumers(consumerAssemblies);
            return configure;
        }

        public static IBusRegistrationConfigurator FormatEndpointName(this IBusRegistrationConfigurator configure, string environmentNamePrefix)
        {
            configure.SetEndpointNameFormatter(new TypeNameEndpointNameFormatter($"{environmentNamePrefix}.", true));
            return configure;
        }
    }
}