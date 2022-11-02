using Architecture;

namespace Project.Infrastructure.IntegrationEvents.Models
{
    public class IntegrationEventTypes
    {
        private IntegrationEventTypes()
        {
            EventTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetLoadableTypes())
                .Where(t => typeof(IntegrationEvent).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass)
                .ToList();
        }

        public List<Type> EventTypes { get; }

        public static IntegrationEventTypes Instance = new IntegrationEventTypes();
    }
}