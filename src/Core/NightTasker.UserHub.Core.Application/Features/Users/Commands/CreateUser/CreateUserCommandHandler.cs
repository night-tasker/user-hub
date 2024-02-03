using MediatR;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.Users.Commands.CreateUser;

internal class CreateUserCommandHandler(
    IUserService userService) : IRequestHandler<CreateUserCommand>
{
    private readonly IUserService _userService =
        userService ?? throw new ArgumentNullException(nameof(userService));
    
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var createUserDto = request.ToCreateUserDto();
        await _userService.CreateUser(createUserDto, cancellationToken);
    }
}