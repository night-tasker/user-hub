﻿using NightTasker.Common.Core.Exceptions.Base;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.Exceptions.NotFound;

public class UserInfoNotFoundException(Guid id) : NotFoundException($"{nameof(UserInfo)} with id {id} not found.");