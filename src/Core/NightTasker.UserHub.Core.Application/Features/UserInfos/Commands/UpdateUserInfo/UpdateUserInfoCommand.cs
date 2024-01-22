using MediatR;

namespace NightTasker.UserHub.Core.Application.Features.UserInfos.Commands.UpdateUserInfo;

/// <summary>
/// Запрос на обновление <see cref="UserInfos"/>
/// </summary>
public class UpdateUserInfoCommand : IRequest<Unit>
{
    /// <summary>ИД пользователя.</summary>
    public Guid Id { get; set; }

    /// <summary>Имя.</summary>
    public string? FirstName { get; set; }

    /// <summary>Отчество.</summary>
    public string? MiddleName { get; set; }

    /// <summary>Фамилия.</summary>
    public string? LastName { get; set; }
}