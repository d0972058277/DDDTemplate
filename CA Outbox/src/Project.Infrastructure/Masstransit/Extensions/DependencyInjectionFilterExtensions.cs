using MassTransit;
using Project.Infrastructure.Masstransit.ConfigurationObservers;

namespace Project.Infrastructure.Masstransit.Extensions
{
    // https://github.com/MassTransit/MassTransit/blob/3b6a9114bc4e776eb1c0e95210dd3d49064f2958/src/MassTransit/Configuration/DependencyInjection/DependencyInjectionFilterExtensions.cs#L15
    public static class DependencyInjectionFilterExtensions
    {
        public static void UseConsumerConsumeFilter(this IConsumePipeConfigurator configurator, Type filterType, IServiceProvider provider)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            var observer = new ScopedConsumerConsumePipeSpecificationObserver(filterType, provider);

            configurator.ConnectConsumerConfigurationObserver(observer);
        }
    }
}