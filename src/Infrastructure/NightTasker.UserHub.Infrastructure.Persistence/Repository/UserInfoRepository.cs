using NightTasker.Common.Core.Persistence.Repository;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.Repository;

public class UserInfoRepository
    (ApplicationDbContext context) : BaseRepository<UserInfo, Guid, ApplicationDbContext>(context), IUserInfoRepository
{
    
}