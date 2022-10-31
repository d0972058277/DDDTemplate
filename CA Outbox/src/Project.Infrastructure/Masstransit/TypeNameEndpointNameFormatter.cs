using System.Text;
using System.Text.RegularExpressions;
using MassTransit;
using MassTransit.Metadata;
using MassTransit.NewIdFormatters;

namespace Project.Infrastructure.Masstransit
{
    // NOTE: https://github.com/MassTransit/MassTransit/blob/c9fc01d3415a7948882977aa6a44e42cde2a1a8d/src/MassTransit/Configuration/DefaultEndpointNameFormatter.cs
    public class TypeNameEndpointNameFormatter : IEndpointNameFormatter
    {
        const int MaxTemporaryQueueNameLength = 72;
        const int OverheadLength = 29;
        static readonly char[] _removeChars = { '.', '+' };
        static readonly Regex _nonAlpha = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled | RegexOptions.Singleline);
        readonly bool _includeNamespace;
        readonly string _prefix;

#nullable disable warnings
        public TypeNameEndpointNameFormatter(bool includeNamespace)
        {
            _includeNamespace = includeNamespace;
        }

        public TypeNameEndpointNameFormatter(string prefix, bool includeNamespace)
        {
            _prefix = prefix;
            _includeNamespace = includeNamespace;
        }

        protected TypeNameEndpointNameFormatter()
        {
            _includeNamespace = false;
        }
#nullable enable warnings

        public static IEndpointNameFormatter Instance { get; } = new TypeNameEndpointNameFormatter();

        public string Separator { get; protected set; } = "";

        public string TemporaryEndpoint(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                tag = "endpoint";

            var host = HostMetadataCache.Host;

            var machineName = _nonAlpha.Replace(host.MachineName!, "");
            var machineNameLength = machineName.Length;

            var processName = _nonAlpha.Replace(host.ProcessName!, "");
            var processNameLength = processName.Length;

            var tagLength = tag.Length;

            var nameLength = machineNameLength + processNameLength + tagLength + OverheadLength;

            var overage = nameLength - MaxTemporaryQueueNameLength;

            const int spread = (MaxTemporaryQueueNameLength - OverheadLength) / 3;

            if (overage > 0 && machineNameLength > spread)
            {
                overage -= machineNameLength - spread;
                machineNameLength = spread;
            }

            if (overage > 0 && processNameLength > spread)
            {
                overage -= processNameLength - spread;
                processNameLength = spread;
            }

            if (overage > 0 && tagLength > spread)
            {
                overage -= tagLength - spread;
                tagLength = spread;
            }

            var sb = new StringBuilder(machineNameLength + processNameLength + tagLength + OverheadLength);

            sb.Append(machineName, 0, machineNameLength);
            sb.Append('_');
            sb.Append(processName, 0, processNameLength);

            sb.Append('_');
            sb.Append(tag, 0, tagLength);
            sb.Append('_');
            sb.Append(NewId.Next().ToString(ZBase32Formatter.LowerCase));

            return sb.ToString();
        }


        public string Consumer<T>()
            where T : class, IConsumer
        {
            return GetConsumerName<T>();
        }

        public string Message<T>()
            where T : class
        {
            return GetMessageName(typeof(T));
        }

        public string Saga<T>()
            where T : class, ISaga
        {
            return GetSagaName<T>();
        }

        public string ExecuteActivity<T, TArguments>()
            where T : class, IExecuteActivity<TArguments>
            where TArguments : class
        {
            var activityName = GetActivityName<T>();

            return $"{activityName}_execute";
        }

        public string CompensateActivity<T, TLog>()
            where T : class, ICompensateActivity<TLog>
            where TLog : class
        {
            var activityName = GetActivityName<T>();

            return $"{activityName}_compensate";
        }

        public virtual string SanitizeName(string name)
        {
            return name;
        }

        string GetConsumerName<T>()
        {
            if (typeof(T).IsGenericType && typeof(T).Name.Contains('`'))
                return SanitizeName(FormatName(typeof(T).GetGenericArguments().Last()));

            const string consumer = "Consumer";

            var consumerName = FormatName(typeof(T));

            if (consumerName.EndsWith(consumer, StringComparison.InvariantCultureIgnoreCase))
                consumerName = consumerName.Substring(0, consumerName.Length - consumer.Length);

            return SanitizeName(consumerName);
        }

        string GetMessageName(Type type)
        {
            if (type.IsGenericType && type.Name.Contains('`'))
                return SanitizeName(FormatName(type.GetGenericArguments().Last()));

            var messageName = type.Name;

            return SanitizeName(messageName);
        }

        string GetSagaName<T>()
        {
            const string saga = "Saga";

            var sagaName = FormatName(typeof(T));

            if (sagaName.EndsWith(saga, StringComparison.InvariantCultureIgnoreCase))
                sagaName = sagaName.Substring(0, sagaName.Length - saga.Length);

            return SanitizeName(sagaName);
        }

        string GetActivityName<T>()
        {
            const string activity = "Activity";

            var activityName = FormatName(typeof(T));

            if (activityName.EndsWith(activity, StringComparison.InvariantCultureIgnoreCase))
                activityName = activityName.Substring(0, activityName.Length - activity.Length);

            return SanitizeName(activityName);
        }

        string FormatName(Type type)
        {
            // NOTE: 調整為
            var name = _includeNamespace ?
                TypeMetadataCache.GetShortName(type) :
                type.Name;

            return string.IsNullOrWhiteSpace(_prefix) ? name : _prefix + name;
        }
    }
}