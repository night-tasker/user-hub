using MediatR;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Users.Models;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Users.Queries.GetById;

internal class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserById(request.UserId, cancellationToken);
        var userDto = UserDto.FromEntity(user);
        return userDto;
    }

    private async Task<User> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.TryGetById(userId, false, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        return user;
    }
}