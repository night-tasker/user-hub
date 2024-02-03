using NightTasker.Common.Core.Exceptions.Base;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class UserNotFoundException(Guid id) : NotFoundException($"{nameof(User)} with id {id} not found.");