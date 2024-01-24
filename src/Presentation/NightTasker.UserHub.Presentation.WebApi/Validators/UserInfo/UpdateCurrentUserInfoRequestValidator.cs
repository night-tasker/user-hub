using FluentValidation;
using NightTasker.UserHub.Presentation.WebApi.Requests.UserInfo;

namespace NightTasker.UserHub.Presentation.WebApi.Validators.UserInfo;

/// <summary>
/// Запрос на обновление <see cref="UserInfo"/> текущего пользователя.
/// </summary>
/// <param name="FirstName">Имя.</param>
/// <param name="MiddleName">Отчество.</param>
/// <param name="LastName">Фамилия.</param>
public class UpdateCurrentUserInfoRequestValidator : AbstractValidator<UpdateCurrentUserInfoRequest>
{
    public UpdateCurrentUserInfoRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
};