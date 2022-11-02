using MassTransit.Configuration;
using MassTransit.RabbitMqTransport;

namespace Project.Infrastructure.Masstransit.Extensions
{
    public static class MessageTopologyConfiguratorExtensions
    {
        public static void FormattEntityName(this IMessageTopologyConfigurator config, string environmentNamePrefix)
        {
            config.SetEntityNameFormatter(new PrefixMessageNameFormatterEntityNameFormatter($"{environmentNamePrefix}.", new RabbitMqMessageNameFormatter()));
        }
    }
}