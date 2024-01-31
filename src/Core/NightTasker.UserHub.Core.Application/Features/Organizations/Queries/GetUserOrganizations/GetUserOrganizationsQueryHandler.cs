using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetUserOrganizations;

public class GetUserOrganizationsQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<GetUserOrganizationsQuery, IReadOnlyCollection<OrganizationDto>>
{
    private readonly IMapper _mapper
        = mapper ?? throw new ArgumentNullException(nameof(mapper));
    private readonly IUnitOfWork _unitOfWork
        = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<IReadOnlyCollection<OrganizationDto>> Handle(
        GetUserOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var organizations = await _unitOfWork.OrganizationRepository.GetUserOrganizations(
            request.UserInfoId, false, cancellationToken);
        
        return _mapper.Map<IReadOnlyCollection<OrganizationDto>>(organizations);
    }
}