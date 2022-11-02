using CorrelationId.Abstractions;
using KnstArchitecture.SequentialGuid;
using Quartz;

namespace Project.Scheduler.Jobs.Decorators
{
    public class CorrelationLogScopeJobDecorator : IDecoratorJob
    {
        private readonly ICorrelationContextFactory _correlationContextFactory;
        private readonly ILogger _logger;
        private readonly IJob _job;

        public CorrelationLogScopeJobDecorator(ICorrelationContextFactory correlationContextFactory, ILogger logger, IJob job)
        {
            _correlationContextFactory = correlationContextFactory;
            _logger = logger;
            _job = job;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var correlationId = context.JobDetail.JobDataMap.GetString("CorrelationId");

            if (string.IsNullOrWhiteSpace(correlationId))
                correlationId = SequentialGuid.NewGuid().ToString();

            _correlationContextFactory.Create(correlationId, "CorrelationId");

            using (_logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
            {
                await _job.Execute(context);
            }
        }
    }
}