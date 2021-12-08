using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Project.Domain.Aggregates.KanbanBoundedContext;

namespace Project.UseCase.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<Mock<FakeEventMediator>>(sp =>
            {
                var mock = new Mock<FakeEventMediator>();

                mock.Setup(m => m.PublishCommandAsync(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                    .Callback<ICommand, CancellationToken>((command, cancellationToken) => mock.Object.Commands.Add(command))
                    .Returns(Task.CompletedTask);

                mock.Setup(m => m.PublishCommandAsync(It.IsAny<ICommand<Guid>>(), It.IsAny<CancellationToken>()))
                    .Callback<ICommand<Guid>, CancellationToken>((command, cancellationToken) => mock.Object.Commands.Add(command))
                    .Returns(Task.FromResult(default(Guid)));

                mock.Setup(m => m.PublishDomainEventAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()))
                    .Callback<IDomainEvent, CancellationToken>((domainEvent, cancellationToken) => mock.Object.DomainEvents.Add(domainEvent))
                    .Returns(Task.CompletedTask);

                mock.Setup(m => m.PublishIntegrationEventAsync(It.IsAny<IntegrationEvent>(), It.IsAny<CancellationToken>()))
                    .Callback<IntegrationEvent, CancellationToken>((integrationEvent, cancellationToken) => mock.Object.IntegrationEvents.Add(integrationEvent))
                    .Returns(Task.CompletedTask);

                mock.Setup(m => m.PublishIntegrationEventsAsync(It.IsAny<IEnumerable<IntegrationEvent>>(), It.IsAny<CancellationToken>()))
                    .Callback<IEnumerable<IntegrationEvent>, CancellationToken>((integrationEvents, cancellationToken) =>
                    {
                        foreach (var integrationEvent in integrationEvents)
                            mock.Object.IntegrationEvents.Add(integrationEvent);
                    })
                    .Returns(Task.CompletedTask);
                return mock;
            });

            services.AddTransient<Mock<FakeBoardRepository>>(sp =>
            {
                var mock = new Mock<FakeBoardRepository>();

                mock.Setup(m => m.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .Returns((Guid aggregateId, CancellationToken cancellationToken) =>
                    {
                        mock.Object.Boards.TryGetValue(aggregateId, out var aggregate);
                        return Task.FromResult(aggregate);
                    });

                mock.Setup(m => m.SaveAsync(It.IsAny<Board>(), It.IsAny<CancellationToken>()))
                .Callback<Board, CancellationToken>((aggregate, cancellationToken) =>
                {
                    if (mock.Object.Boards.ContainsKey(aggregate.Id))
                        mock.Object.Boards[aggregate.Id] = aggregate;
                    else
                        mock.Object.Boards.Add(aggregate.Id, aggregate);
                });

                return mock;
            });
        }
    }
}