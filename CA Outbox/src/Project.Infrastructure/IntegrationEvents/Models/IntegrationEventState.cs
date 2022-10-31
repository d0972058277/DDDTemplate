namespace Project.Infrastructure.IntegrationEvents.Models
{
    public enum IntegrationEventState
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}