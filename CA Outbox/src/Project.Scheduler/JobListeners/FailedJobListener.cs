using Quartz;

namespace Project.Scheduler.JobListeners
{
    public class FailedJobListener : IJobListener
    {
        private readonly ILogger<FailedJobListener> _logger;

        public FailedJobListener(ILogger<FailedJobListener> logger)
        {
            _logger = logger;
        }

        public string Name => nameof(FailedJobListener);

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = default)
        {
            if (jobException is null)
                return Task.CompletedTask;

            var correlationId = context.JobDetail.JobDataMap.GetString("CorrelationId");

            using (_logger.BeginScope(new Dictionary<string, object?> { ["CorrelationId"] = correlationId }))
            {
                _logger.LogWarning("Job with ID and type: {JobId}, {JobType} has thrown the exception: {@JobException}.", context.JobDetail.Key, context.JobDetail.JobType, jobException);
                return Task.CompletedTask;
            }
        }
    }
}