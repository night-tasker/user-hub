using MassTransit;
using MediatR;
using NightTasker.Common.Messaging.Events.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.CreateUserInfo;

namespace NightTasker.UserHub.Infrastructure.Messaging.Consumers.Events.User;

public class UserRegisteredConsumer(
    ISender sender) : IConsumer<IUserRegistered>
{
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    public Task Consume(ConsumeContext<IUserRegistered> context)
    {
        var createUserInfoCommand = new CreateUserInfoCommand(
            context.Message.Id, 
            context.Message.UserName,
            context.Message.Email);
        return _sender.Send(createUserInfoCommand, context.CancellationToken);
    }
}