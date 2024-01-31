using FluentValidation;
using NightTasker.UserHub.Presentation.WebApi.Requests.UserInfo;

namespace NightTasker.UserHub.Presentation.WebApi.Validators.UserInfo;

public class UpdateCurrentUserInfoRequestValidator : AbstractValidator<UpdateCurrentUserInfoRequest>
{
    public UpdateCurrentUserInfoRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}