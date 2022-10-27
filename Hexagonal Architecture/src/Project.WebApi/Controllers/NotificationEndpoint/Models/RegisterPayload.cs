using FluentValidation;
using Project.Domain.Aggregates.NotificationAggregate;

namespace Project.WebApi.Controllers.NotificationEndpoint.Models
{
    public class RegisterPayload
    {
        public MessageDto Message { get; set; } = default!;
        public DateTime Schedule { get; set; }
        public List<Guid> DeviceIds { get; set; } = new();

        public class MessageDto
        {
            public string Title { get; set; } = default!;
            public string Body { get; set; } = default!;
        }
    }

    public class RegisterPayloadValidator : AbstractValidator<RegisterPayload>
    {
        public RegisterPayloadValidator()
        {
            RuleFor(payload => payload.Message).Custom((value, context) =>
            {
                var result = Message.Create(value.Title, value.Body);
                if (result.IsFailure)
                    context.AddFailure(result.Error);
            });

            RuleFor(payload => payload.Schedule).Custom((value, context) =>
            {
                var result = Schedule.Create(value);
                if (result.IsFailure)
                    context.AddFailure(result.Error);
            });
        }
    }


}