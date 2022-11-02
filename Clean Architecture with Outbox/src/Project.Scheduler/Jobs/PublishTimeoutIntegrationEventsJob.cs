using Architecture;
using Quartz;

namespace Project.Scheduler.Jobs
{
    [DisallowConcurrentExecution]
    public class PublishTimeoutIntegrationEventsJob : IGeneralJob
    {
        private readonly IIntegrationEventOutbox _integrationEventOutbox;

        public PublishTimeoutIntegrationEventsJob(IIntegrationEventOutbox integrationEventOutbox)
        {
            _integrationEventOutbox = integrationEventOutbox;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _integrationEventOutbox.PublishTimeoutEventsAsync(context.CancellationToken);
        }
    }
}