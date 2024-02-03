using FluentValidation;
using NightTasker.UserHub.Presentation.WebApi.Requests.User;

namespace NightTasker.UserHub.Presentation.WebApi.Validators.User;

public class UpdateCurrentUserRequestValidator : AbstractValidator<UpdateCurrentUserRequest>
{
    public UpdateCurrentUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();
    }
}