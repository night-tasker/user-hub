namespace NightTasker.UserHub.Infrastructure.Grpc.Settings;

public class StorageGrpcSettings : GrpcSettings
{
    public string? BucketName { get; init; }
}