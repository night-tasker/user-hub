namespace NightTasker.UserHub.Infrastructure.Grpc.Settings;

/// <summary>
/// Настройки для gRPC для Storage.
/// </summary>
public class StorageGrpcSettings : GrpcSettings
{
    /// <summary>
    /// Название хранилища.
    /// </summary>
    public string? BucketName { get; set; }
}