using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using KingnetSmartlife.DDD.CleanArchitecture.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Project.Application.Repositories.KanbanBoundedContext;
using Project.Infrastructure.PipelineBehaviors;
using Project.Infrastructure.Repositories.KanbanBoundedContext;

namespace Project.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProject(this IServiceCollection services)
        {
            services.AddSingleton<FakeDbContext>();

            services.TryAddTransient<IBoardRepository, BoardRepository>();

            services.TryAddTransient<IWorkflowRepository, WorkflowRepository>();

            services.TryAddTransient<IListBoardsQueryRepository, ListBoardsQueryRepository>();

            services.TryAddTransient<IEventMediator, EventMediator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), ServiceLifetime.Transient);

            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}