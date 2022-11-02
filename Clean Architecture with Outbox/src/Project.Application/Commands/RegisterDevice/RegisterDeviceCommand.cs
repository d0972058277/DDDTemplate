using Architecture;
using Project.Domain.Aggregates.DeviceAggregate;

namespace Project.Application.Commands.RegisterDevice
{
    public class RegisterDeviceCommand : ICommand<Guid>
    {
        public RegisterDeviceCommand(Token token)
        {
            Token = token;
        }

        public Token Token { get; }
    }
}