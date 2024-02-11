using NightTasker.Common.Core.Abstractions;

namespace NightTasker.UserHub.Core.Domain.Entities;

public class User : IEntityWithId<Guid>, IDateTimeOffsetModification
{
    private User(
        Guid id,
        string? userName = null,
        string? email = null,
        string? firstName = null,
        string? middleName = null,
        string? lastName = null)
    {
        Id = id;
        UserName = userName;
        Email = email;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }
    
    public static User CreateInstance(
        Guid id,
        string? userName = null,
        string? email = null,
        string? firstName = null,
        string? middleName = null,
        string? lastName = null)
    {
        return new User(id, userName, email, firstName, middleName, lastName);
    }
    
    public Guid Id { get; private set; }

    public string? UserName { get; private set; }

    public string? Email { get; private set; }

    public string? FirstName { get; private set; }

    public string? MiddleName { get; private set; }
    
    public string? LastName { get; private set; }

    public List<UserImage> UserImages { get; } = null!;

    public List<OrganizationUser> OrganizationUsers { get; } = null!;

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