using Architecture;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Project.Domain.Exceptions;

namespace Project.Infrastructure.MediatR.PipelineBehaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidatorBehavior(ILogger<ValidatorBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
        {
            _logger = logger;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Validating request {RequestType}", typeName);

            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {RequestType} - Request: {@Request} - Errors: {@ValidationErrors}", typeName, request, failures);

                throw new ValidationDomainException(
                    $"Request Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}