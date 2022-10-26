using FluentValidation;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.WebApi.Controllers.DeviceEndpoint.Models
{
    public class RegisterPayload
    {
        public string Token { get; set; } = default!;
    }

    public class RegisterPayloadValidator : AbstractValidator<RegisterPayload>
    {
        public RegisterPayloadValidator()
        {
            RuleFor(payload => payload.Token).Custom((value, context) =>
            {
                var result = Token.Create(value);
                if (result.IsFailure)
                    context.AddFailure(result.Error);
            });
        }
    }
}