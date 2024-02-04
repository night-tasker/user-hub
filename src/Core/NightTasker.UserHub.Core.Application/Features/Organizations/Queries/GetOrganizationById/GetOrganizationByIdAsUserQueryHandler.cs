using MediatR;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

internal class GetOrganizationByIdAsUserQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetOrganizationByIdAsUserQuery, OrganizationWithInfoDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<OrganizationWithInfoDto> Handle(GetOrganizationByIdAsUserQuery request, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetOrganizationForUser(
            request.UserId, request.OrganizationId, false, cancellationToken);

        if (organization is null)
        {
            throw new OrganizationUserNotFoundException(request.OrganizationId, request.UserId);   
        }
        
        return await OrganizationWithInfoDto.FromEntity(
            organization, _unitOfWork.OrganizationUserRepository, cancellationToken);
    }
}