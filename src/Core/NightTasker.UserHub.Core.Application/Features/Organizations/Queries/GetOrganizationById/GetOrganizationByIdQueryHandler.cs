using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Models.Organization;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Хэндлер для <see cref="GetOrganizationByIdQuery"/>
/// </summary>
internal class GetOrganizationByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetOrganizationByIdQuery, OrganizationWithInfoDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<OrganizationWithInfoDto> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetOrganizationWithInfo(
            request.OrganizationId, cancellationToken);

        if (organization is null)
        {
            throw new OrganizationNotFoundException(request.OrganizationId);   
        }
        
        return organization;
    }
}