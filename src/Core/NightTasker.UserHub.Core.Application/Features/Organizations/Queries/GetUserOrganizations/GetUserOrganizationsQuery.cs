﻿using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;

public record GetUserOrganizationsQuery(Guid UserInfoId) : IRequest<IReadOnlyCollection<OrganizationDto>>;