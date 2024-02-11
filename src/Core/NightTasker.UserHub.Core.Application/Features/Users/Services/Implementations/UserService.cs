using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Users.Models;
using NightTasker.UserHub.Core.Application.Features.Users.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Users.Services.Implementations;

public class UserService(
    IUnitOfWork unitOfWork) : IUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task CreateUser(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var user = createUserDto.ToEntity();
        await _unitOfWork.UserRepository.Add(user, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    public async Task UpdateUser(UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var user = await GetUserById(updateUserDto.Id, true, cancellationToken);
        updateUserDto.MapFieldsToEntity(user);
        await _unitOfWork.SaveChanges(cancellationToken);
    }

    private async Task<User> GetUserById(
        Guid userId, bool trackChanges, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.TryGetById(userId, trackChanges, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        return user;
    }
}