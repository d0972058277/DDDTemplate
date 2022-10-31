using Architecture;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Project.Infrastructure.MediatR.PipelineBehaviors
{
    public class QueryBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<QueryBehaviour<TRequest, TResponse>> _logger;

        public QueryBehaviour(ILogger<QueryBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is IBaseQuery)
                return await HandleQuery(request, cancellationToken, next);
            else
                return await next();
        }

        private async Task<TResponse> HandleQuery(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await PassNoTransaction(request, cancellationToken, next);
            }
            catch (Exception ex)
            {
                var typeName = request.GetGenericTypeName();

                if (cancellationToken.IsCancellationRequested)
                    _logger.LogInformation(ex, "ERROR Handling no transaction for {QueryName} ({@Query})", typeName, request);
                else
                    _logger.LogError(ex, "ERROR Handling no transaction for {QueryName} ({@Query})", typeName, request);

                throw;
            }
        }

        private async Task<TResponse> PassNoTransaction(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Use Readonly DbContext for {QueryName} ({@Query})", typeName, request);

            var response = await next();

            return response;
        }
    }
}