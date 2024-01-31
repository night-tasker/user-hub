namespace NightTasker.UserHub.Presentation.WebApi.Settings;

public class IdentitySettings
{
    public string SecretKey { get; set; } = null!;

    public string EncryptKey { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;
}