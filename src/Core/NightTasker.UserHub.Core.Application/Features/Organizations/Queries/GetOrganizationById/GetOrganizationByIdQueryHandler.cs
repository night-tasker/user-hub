using MapsterMapper;
using MediatR;
using NightTasker.UserHub.Core.Application.ApplicationContracts.Repository;
using NightTasker.UserHub.Core.Application.Exceptions.NotFound;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;

namespace NightTasker.UserHub.Core.Application.Features.Organizations.Queries.GetOrganizationById;

/// <summary>
/// Хэндлер для <see cref="GetOrganizationByIdQuery"/>
/// </summary>
public class GetOrganizationByIdQueryHandler(
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

    public async Task<OrganizationDto> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
    {
        var organization = await _unitOfWork.OrganizationRepository.TryGetById(
            request.OrganizationId, false, cancellationToken);

        if (organization is null)
        {
            throw new OrganizationNotFoundException(request.OrganizationId);   
        }
        
        return _mapper.Map<OrganizationDto>(organization);
    }
}