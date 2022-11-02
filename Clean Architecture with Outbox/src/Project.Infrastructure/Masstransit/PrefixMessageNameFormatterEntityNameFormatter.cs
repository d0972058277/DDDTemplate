using MassTransit;
using MassTransit.Transports;

namespace Project.Infrastructure.Masstransit
{
    public class PrefixMessageNameFormatterEntityNameFormatter : IEntityNameFormatter
    {
        readonly string _prefix;
        readonly IMessageNameFormatter _formatter;

        public PrefixMessageNameFormatterEntityNameFormatter(string prefix, IMessageNameFormatter formatter)
        {
            _prefix = prefix;
            _formatter = formatter;
        }

        string IEntityNameFormatter.FormatEntityName<T>()
        {
            return _prefix + _formatter.GetMessageName(typeof(T)).ToString();
        }
    }
}