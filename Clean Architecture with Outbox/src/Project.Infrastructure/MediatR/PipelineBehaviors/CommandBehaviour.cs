using Architecture;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Project.Infrastructure.MediatR.PipelineBehaviors
{
    public class CommandBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ProjectDbContext _dbContext;
        private readonly IIntegrationEventOutbox<IDbContextTransaction> _integrationEventOutbox;
        private readonly ILogger<CommandBehaviour<TRequest, TResponse>> _logger;

        public CommandBehaviour(ProjectDbContext dbContext, IIntegrationEventOutbox<IDbContextTransaction> integrationEventOutbox, ILogger<CommandBehaviour<TRequest, TResponse>> logger)
        {
            _dbContext = dbContext;
            _integrationEventOutbox = integrationEventOutbox;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IBaseCommand)
                return await HandleCommand(request, cancellationToken, next);
            else
                return await next();
        }

        private async Task<TResponse> HandleCommand(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                if (_dbContext.HasActiveTransaction)
                    return await ContinueTransaction(request, cancellationToken, next);
                else
                    return await BeginTransaction(request, cancellationToken, next);
            }
            catch (Exception ex)
            {
                var typeName = request.GetGenericTypeName();

                if (cancellationToken.IsCancellationRequested)
                    _logger.LogInformation(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);
                else
                    _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }

        private async Task<TResponse> BeginTransaction(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            var response = await strategy.ExecuteAsync(async () =>
            {
                _dbContext.Initialize();
                using var transaction = await _dbContext.BeginTransactionAsync(cancellationToken);

                var typeName = request.GetGenericTypeName();
                var transactionId = transaction!.TransactionId;

                _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transactionId, typeName, request);

                var result = await next();

                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                await _dbContext.SaveChangesAsync(cancellationToken);

                await _dbContext.CommitAsync(transaction, cancellationToken);

                sw.Stop();
                _logger.LogInformation("----- SaveChangesAsync {TransactionId} for {CommandName} costs {ElapsedMilliseconds}ms", transactionId, typeName, sw.ElapsedMilliseconds);

                _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transactionId, typeName);

                await _integrationEventOutbox.PublishEventsAsync(transaction, cancellationToken);

                return result;
            });

            return response;
        }

        private async Task<TResponse> ContinueTransaction(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();
            var transactionId = _dbContext.CurrentTransaction.TransactionId;

            _logger.LogInformation("----- Continue transaction {TransactionId} for {CommandName} ({@Command})", transactionId, typeName, request);

            var response = await next();

            return response;
        }
    }
}