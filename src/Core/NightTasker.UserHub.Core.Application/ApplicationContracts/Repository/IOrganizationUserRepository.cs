using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;

/// <summary>
/// Репозиторий для работы с пользователями организации (<see cref="OrganizationUser"/>).
/// </summary>
public interface IOrganizationUserRepository : IRepository<OrganizationUser, Guid>;