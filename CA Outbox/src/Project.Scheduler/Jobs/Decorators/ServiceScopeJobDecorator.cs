using Quartz;

namespace Project.Scheduler.Jobs.Decorators
{
    public class ServiceScopeJobDecorator : IDecoratorJob, IDisposable
    {
        private IServiceScope _serviceScope;
        private IJob _job;

        public ServiceScopeJobDecorator(IServiceScope serviceScope, IJob job)
        {
            _serviceScope = serviceScope;
            _job = job;
        }

        public void Dispose()
        {
            _serviceScope.Dispose();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return _job.Execute(context);
        }
    }
}