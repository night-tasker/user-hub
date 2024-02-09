using MediatR;
using NightTasker.UserHub.Core.Application.Features.UserImages.Services.Contracts;
using NightTasker.UserHub.Core.Domain.Repositories;

namespace NightTasker.UserHub.Core.Application.Features.UserImages.Commands.SetActiveUserImage;

internal class SetActiveImageForUserCommandHandler(IUserImageService userImageService, IUnitOfWork unitOfWork)
    : IRequestHandler<SetActiveImageForUserCommand>
{
    private readonly IUserImageService _userImageService = 
        userImageService ?? throw new ArgumentNullException(nameof(userImageService));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task Handle(SetActiveImageForUserCommand request, CancellationToken cancellationToken)
    {
        await _userImageService.SetActiveUserImageForUser(
            request.UserId, request.UserImageId, cancellationToken);
        await _unitOfWork.SaveChanges(cancellationToken);
    }
}