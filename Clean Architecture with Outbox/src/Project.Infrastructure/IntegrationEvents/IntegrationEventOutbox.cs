using Architecture;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Project.Infrastructure.IntegrationEvents.Models;

namespace Project.Infrastructure.IntegrationEvents
{
    public class IntegrationEventOutbox : IIntegrationEventOutbox<IDbContextTransaction>
    {
        private readonly ProjectDbContext _dbContext;
        private readonly IInetgrationEventPublisher _publisher;
        private readonly ILogger<IntegrationEventOutbox> _logger;

        public IntegrationEventOutbox(ProjectDbContext dbContext, IInetgrationEventPublisher publisher, ILogger<IntegrationEventOutbox> logger)
        {
            _dbContext = dbContext;
            _publisher = publisher;
            _logger = logger;
        }

        public Task AddEventAsync(IDbContextTransaction dbContextTransaction, IntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            if (!_dbContext.SameActiveTransaction(dbContextTransaction))
                throw new ArgumentNullException("DomainDbContext.CurrentTransaction");

            var transactionId = dbContextTransaction.TransactionId;
            _dbContext.IntegrationEvents.Add(IntegrationEventEntry.Create(integrationEvent, transactionId));
            return Task.CompletedTask;
        }

        public async Task PublishEventsAsync(IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken)
        {
            var integrationEventEntries = await RetrieveEventsPendingToPublishAsync(dbContextTransaction, cancellationToken);
            await PublishEventsAsync(integrationEventEntries, cancellationToken);
        }

        public async Task PublishTimeoutEventsAsync(CancellationToken cancellationToken)
        {
            var integrationEventEntries = await RetrieveTimeoutEventsPendingToPublishAsync(cancellationToken);
            await PublishEventsAsync(integrationEventEntries, cancellationToken);
        }

        private async Task<IEnumerable<IntegrationEventEntry>> RetrieveEventsPendingToPublishAsync(IDbContextTransaction dbContextTransaction, CancellationToken cancellationToken)
        {
            var transactionId = dbContextTransaction.TransactionId;
            var integrationEvents = await _dbContext.IntegrationEvents
                .Where(e => e.TransactionId == transactionId && e.State == IntegrationEventState.NotPublished)
                .ToListAsync();

            return integrationEvents.Select(e => e.DeserializeJsonContent()).OrderBy(o => o.CreationTimestamp).ToList();
        }

        private async Task<IEnumerable<IntegrationEventEntry>> RetrieveTimeoutEventsPendingToPublishAsync(CancellationToken cancellationToken)
        {
            var timeoutRangeStart = DateTime.UtcNow.AddMinutes(-2);
            var timeoutRangeEnd = DateTime.UtcNow.AddMinutes(-1);
            var timeoutRangeState = new List<IntegrationEventState>
            {
                IntegrationEventState.NotPublished,
                IntegrationEventState.InProgress,
                IntegrationEventState.PublishedFailed
            };

            var integrationEvents = await _dbContext.IntegrationEvents
                .Where(e => e.CreationTimestamp >= timeoutRangeStart && e.CreationTimestamp <= timeoutRangeEnd)
                .Where(e => timeoutRangeState.Contains(e.State))
                .ToListAsync();

            return integrationEvents.Select(e => e.DeserializeJsonContent()).OrderBy(o => o.CreationTimestamp).ToList();
        }

        private async Task PublishEventsAsync(IEnumerable<IntegrationEventEntry> integrationEventEntries, CancellationToken cancellationToken)
        {
            foreach (var entry in integrationEventEntries)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventName} {IntegrationEventId} - ({@IntegrationEvent})", entry.EventTypeShortName, entry.EventId, entry.IntegrationEvent);

                try
                {
                    await MarkEventAsInProgressAsync(entry.EventId, cancellationToken);
                    await _publisher.PublishAsync(entry.IntegrationEvent, cancellationToken);
                    await MarkEventAsPublishedAsync(entry.EventId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId}", entry.EventId);
                    await MarkEventAsFailedAsync(entry.EventId, cancellationToken);
                }
            }
        }

        private Task MarkEventAsFailedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.PublishedFailed, cancellationToken);
        }

        private Task MarkEventAsInProgressAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.InProgress, cancellationToken);
        }

        private Task MarkEventAsPublishedAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return UpdateEventStatus(eventId, IntegrationEventState.Published, cancellationToken);
        }

        private async Task UpdateEventStatus(Guid eventId, IntegrationEventState status, CancellationToken cancellationToken)
        {
            var entry = (await _dbContext.IntegrationEvents.FindAsync(new object[] { eventId }, cancellationToken))!;
            entry.State = status;

            if (status == IntegrationEventState.InProgress)
                entry.TimesSent++;

            _dbContext.IntegrationEvents.Update(entry);

            _logger.LogInformation("----- Updated integration event state: {IntegrationEventId} - ({@IntegrationEvent})", eventId, entry);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}