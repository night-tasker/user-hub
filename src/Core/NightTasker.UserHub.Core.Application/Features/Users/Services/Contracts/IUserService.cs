using NightTasker.UserHub.Core.Application.Features.Users.Models;

namespace NightTasker.UserHub.Core.Application.Features.Users.Services.Contracts;

public interface IUserService
{
    Task CreateUser(CreateUserDto createUserDto, CancellationToken cancellationToken);

    Task UpdateUser(UpdateUserDto updateUserDto, CancellationToken cancellationToken);
}