namespace NightTasker.UserHub.Presentation.WebApi.Endpoints;

public class UserImageEndpoints
{
    public const string UserImageResource = "user-images";

    public const string CurrentUserImage = "current-user";
    
    public const string UploadCurrentUserImage = "current-user/upload";

    public const string GetCurrentUserActiveImageUrl = "current-user/active/url";
    
    public const string GetCurrentUserImagesUrl = "current-user/url";
    
    public const string SetActiveUserImage = "current-user/active/{userImageId}";
    
    public const string RemoveCurrentUserImageById = "current-user/{userImageId}";
}