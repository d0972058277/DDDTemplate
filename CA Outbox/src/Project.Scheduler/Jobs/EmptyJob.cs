using Quartz;

namespace Project.Scheduler.Jobs
{
    public class EmptyJob : IGeneralJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}