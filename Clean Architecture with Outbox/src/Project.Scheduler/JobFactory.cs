using CorrelationId.Abstractions;
using Project.Scheduler.Jobs;
using Project.Scheduler.Jobs.Decorators;
using Quartz;
using Quartz.Spi;

namespace Project.Scheduler
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<JobFactory> _logger;

        public JobFactory(IServiceProvider serviceProvider, ILogger<JobFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var scope = _serviceProvider.CreateScope();
                var correlationContextFactory = scope.ServiceProvider.GetRequiredService<ICorrelationContextFactory>();
                var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

                var job = (IJob)scope.ServiceProvider.GetRequiredService(bundle.JobDetail.JobType);
                job = new CorrelationLogScopeJobDecorator(correlationContextFactory, loggerFactory.CreateLogger(bundle.JobDetail.JobType), job);
                job = new ServiceScopeJobDecorator(scope, job);

                return job;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Exception creating {JobName}. Giving up and returning a do-nothing empty job.", bundle.JobDetail.JobType.Name);
                return new EmptyJob();
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}