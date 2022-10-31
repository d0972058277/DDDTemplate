using MassTransit;
using MassTransit.Configuration;
using MassTransit.DependencyInjection;
using MassTransit.Internals;
using MassTransit.Middleware;

namespace Project.Infrastructure.Masstransit.ConfigurationObservers
{
    // https://github.com/MassTransit/MassTransit/blob/3b6a9114bc4e776eb1c0e95210dd3d49064f2958/src/MassTransit/DependencyInjection/Configuration/ScopedConsumePipeSpecificationObserver.cs#L9
    public class ScopedConsumerConsumePipeSpecificationObserver : IConsumerConfigurationObserver
    {
        private readonly Type _filterType;
        private readonly IServiceProvider _provider;

        public ScopedConsumerConsumePipeSpecificationObserver(Type filterType, IServiceProvider provider)
        {
            _filterType = filterType;
            _provider = provider;
        }

        public void ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator) where TConsumer : class { }

        public void ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
            where TConsumer : class
            where TMessage : class
        {
            if (!_filterType.IsGenericType || !_filterType.IsGenericTypeDefinition)
                throw new ConfigurationException("The scoped filter must be a generic type definition");

            var filterType = _filterType.MakeGenericType(typeof(TConsumer), typeof(TMessage));

            if (!filterType.HasInterface(typeof(IFilter<ConsumerConsumeContext<TConsumer, TMessage>>)))
                throw new ConfigurationException($"The scoped filter must implement {TypeCache<IFilter<ConsumerConsumeContext<TConsumer, TMessage>>>.ShortName} ");


            var scopeProviderType = typeof(FilterScopeProvider<,>).MakeGenericType(filterType, typeof(ConsumerConsumeContext<TConsumer, TMessage>));

            var scopeProvider = (IFilterScopeProvider<ConsumerConsumeContext<TConsumer, TMessage>>)Activator.CreateInstance(scopeProviderType, _provider)!;

            var scopedFilterType = typeof(ScopedFilter<>).MakeGenericType(typeof(ConsumerConsumeContext<TConsumer, TMessage>));

            var filter = (IFilter<ConsumerConsumeContext<TConsumer, TMessage>>)Activator.CreateInstance(scopedFilterType, scopeProvider)!;

            var specification = new FilterPipeSpecification<ConsumerConsumeContext<TConsumer, TMessage>>(filter);

            configurator.AddPipeSpecification(specification);
        }
    }
}