using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;

public class UpdateUserInfoCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    public string? LastName { get; set; }
}