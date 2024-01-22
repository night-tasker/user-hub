using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Features.Organizations.Services.Contracts;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Commands.CreateOrganization;

/// <summary>
/// Хэндлер для <see cref="CreateOrganizationCommand"/>
/// </summary>
internal class CreateOrganizationCommandHandler(
    IOrganizationService organizationService,
    IMapper mapper)
    : IRequestHandler<CreateOrganizationCommand, Guid>
{
    private readonly IOrganizationService _organizationService = 
        organizationService ?? throw new ArgumentNullException(nameof(organizationService));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var createOrganizationDto = _mapper.Map<CreateOrganizationDto>(request);
        return _organizationService.CreateOrganization(createOrganizationDto, cancellationToken);
    }
}