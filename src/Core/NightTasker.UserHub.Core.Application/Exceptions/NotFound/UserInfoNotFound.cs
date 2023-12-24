using NightTasker.Common.Core.Exceptions.Base;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class UserInfoNotFoundException(Guid id) : NotFoundException($"User info with id {id} not found.");