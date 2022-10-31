using System.Diagnostics;
using Architecture;
using KnstArchitecture.SequentialGuid;
using MassTransit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Project.Infrastructure.Masstransit.PipelineBehaviors
{
    public class LoggingBehavior<TMessage> : IFilter<ConsumeContext<TMessage>> where TMessage : IntegrationEvent
    {
        private ILogger<LoggingBehavior<TMessage>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TMessage>> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(ConsumeContext<TMessage> context, IPipe<ConsumeContext<TMessage>> next)
        {
            var property = new Dictionary<string, object?>();

            property.Add("MessageId", context.MessageId);
            property.Add("CorrelationId", context.CorrelationId);
            property.Add("ConsumeId", SequentialGuid.NewGuid());

            using (var scope = _logger.BeginScope(property))
            {
                var integrationEventId = context.Message.Id;
                var integrationEventName = context.Message.GetType().ShortDisplayName();
                var sw = new Stopwatch();
                try
                {
                    _logger.LogInformation("----- Begin consuming integration event {IntegrationEventName} for {IntegrationEventId} ({@Message})", integrationEventName, integrationEventId, context.Message);
                    sw.Start();

                    await next.Send(context);

                    sw.Stop();
                    _logger.LogInformation("----- Done consuming integration event {IntegrationEventName} for {IntegrationEventId} in {@Elapsed} ms", integrationEventName, integrationEventId, sw.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    sw.Stop();
                    _logger.LogError(e, "----- Fail consuming integration event {IntegrationEventName} for {IntegrationEventId} in {@Elapsed} ms", integrationEventName, integrationEventId, sw.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }

    // https://bartwullems.blogspot.com/2020/09/masstransitreading-header-using.html
    public class LoggingBehavior<TConsumer, TMessage> : IFilter<ConsumerConsumeContext<TConsumer, TMessage>>
        where TConsumer : class
        where TMessage : IntegrationEvent
    {
        private readonly ILogger<LoggingBehavior<TConsumer, TMessage>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TConsumer, TMessage>> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context) { }

        public async Task Send(ConsumerConsumeContext<TConsumer, TMessage> context, IPipe<ConsumerConsumeContext<TConsumer, TMessage>> next)
        {
            var property = new Dictionary<string, object?>();

            property.Add("MessageId", context.MessageId);
            property.Add("CorrelationId", context.CorrelationId);
            property.Add("Consumer", context.Consumer.GetType().FullName);
            property.Add("ConsumeId", SequentialGuid.NewGuid());

            using (var scope = _logger.BeginScope(property))
            {
                var integrationEventId = context.Message.Id;
                var integrationEventName = context.Message.GetType().ShortDisplayName();
                var sw = new Stopwatch();
                try
                {
                    _logger.LogInformation("----- Begin consuming integration event {IntegrationEventName} for {IntegrationEventId} ({@Message})", integrationEventName, integrationEventId, context.Message);
                    sw.Start();

                    await next.Send(context);

                    sw.Stop();
                    _logger.LogInformation("----- Done consuming integration event {IntegrationEventName} for {IntegrationEventId} in {@Elapsed} ms", integrationEventName, integrationEventId, sw.ElapsedMilliseconds);
                }
                catch (Exception e)
                {
                    sw.Stop();
                    _logger.LogError(e, "----- Fail consuming integration event {IntegrationEventName} for {IntegrationEventId} in {@Elapsed} ms", integrationEventName, integrationEventId, sw.ElapsedMilliseconds);
                    throw;
                }
            }
        }
    }
}