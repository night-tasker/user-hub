using MassTransit;
using MediatR;
using NightTasker.Common.Messaging.Events.Contracts;
using NightTasker.UserHub.Core.Application.Features.UserInfo.Commands.CreateUserInfo;

namespace NightTasker.UserHub.Infrastructure.Messaging.Consumers.Events.User;

/// <summary>
/// Консьюмер для события <see cref="IUserRegistered"/>.
/// </summary>
/// <param name="sender">Сэндер запросов.</param>
public class UserRegisteredConsumer(
    ISender sender) : IConsumer<IUserRegistered>
{
    private readonly ISender _sender = sender ?? throw new ArgumentNullException(nameof(sender));

    public Task Consume(ConsumeContext<IUserRegistered> context)
    {
        var createUserInfoCommand = new CreateUserInfoCommand(context.Message.Id, context.Message.UserName);
        return _sender.Send(createUserInfoCommand, context.CancellationToken);
    }
}