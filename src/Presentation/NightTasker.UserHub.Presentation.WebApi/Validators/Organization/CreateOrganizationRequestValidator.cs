using FluentValidation;
using NightTasker.UserHub.Presentation.WebApi.Requests.Organization;

namespace NightTasker.UserHub.Presentation.WebApi.Validators.Organization;

/// <summary>
/// Валидатор для <see cref="CreateOrganizationRequest"/>.
/// </summary>
public class CreateOrganizationRequestValidator : AbstractValidator<CreateOrganizationRequest>
{
    public CreateOrganizationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}