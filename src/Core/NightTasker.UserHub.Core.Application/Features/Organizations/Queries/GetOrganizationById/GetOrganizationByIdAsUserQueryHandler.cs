using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Models.Organization;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

internal class GetOrganizationByIdAsUserQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetOrganizationByIdAsUserQuery, OrganizationWithInfoDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<OrganizationWithInfoDto> Handle(GetOrganizationByIdAsUserQuery request, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetOrganizationWithInfoForUser(
            request.OrganizationId, request.UserId, cancellationToken);

        if (organization is null)
        {
            throw new OrganizationNotFoundException(request.OrganizationId);   
        }
        
        return organization;
    }
}