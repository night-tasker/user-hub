using MediatR;
using NightTasker.UserHub.Core.Application.Features.Users.Models;

namespace NightTasker.UserHub.Core.Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;