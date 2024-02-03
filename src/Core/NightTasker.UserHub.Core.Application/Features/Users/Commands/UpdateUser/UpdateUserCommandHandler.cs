using MediatR;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.Users.Commands.UpdateUser;

internal class UpdateUserCommandHandler(
    IUserService userService) : IRequestHandler<UpdateUserCommand, Unit>
{
    private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));

    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var updateUserDto = request.ToUpdateUserDto();
        await _userService.UpdateUser(updateUserDto, cancellationToken);
        return Unit.Value;
    }
}