using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class UserInfo : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    public Guid Id { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }

    public List<UserImage> UserInfoImages { get; set; } = null!;

    public List<OrganizationUser> OrganizationUsers { get; set; } = null!;

    public DateTimeOffset CreatedDateTimeOffset { get; set; }
    
    public DateTimeOffset? UpdatedDateTimeOffset { get; set; }

    public void UpdateUserName(string? userName)
    {
        UserName = userName;
    }

    public void UpdateEmail(string? email)
    {
        Email = email;
    }

    public void UpdateFirstName(string? firstName)
    {
        FirstName = firstName;
    }

    public void UpdateMiddleName(string? middleName)
    {
        MiddleName = middleName;
    }

    public void UpdateLastName(string? lastName)
    {
        LastName = lastName;
    }
}