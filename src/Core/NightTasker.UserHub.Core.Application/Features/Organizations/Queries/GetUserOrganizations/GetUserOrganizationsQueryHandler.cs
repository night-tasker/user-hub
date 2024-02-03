using MediatR;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;

public class GetUserOrganizationsQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserOrganizationsQuery, IReadOnlyCollection<OrganizationDto>>
{
    private readonly IUnitOfWork _unitOfWork
        = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<IReadOnlyCollection<OrganizationDto>> Handle(
        GetUserOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = await _unitOfWork.OrganizationRepository.GetUserOrganizations(
            request.UserId, false, cancellationToken);
        
        return OrganizationDto.FromEntities(organizations).ToList();
    }
}