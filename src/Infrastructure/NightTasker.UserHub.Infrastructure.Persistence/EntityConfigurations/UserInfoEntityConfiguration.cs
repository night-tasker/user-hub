using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="UserInfo"/>
/// </summary>
public class UserInfoEntityConfiguration : IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable(name: "user_infos");
        
        builder.HasKey(userInfo => userInfo.Id);

        builder.Property(userInfo => userInfo.UserName)
            .HasMaxLength(32);

        builder.Property(userInfo => userInfo.FirstName)
            .HasMaxLength(32);
        
        builder.Property(userInfo => userInfo.MiddleName)
            .HasMaxLength(32);
        
        builder.Property(userInfo => userInfo.LastName)
            .HasMaxLength(32);
        
        builder.Property(userInfo => userInfo.Email)
            .HasMaxLength(254);

        builder.Property(userInfo => userInfo.CreatedDateTimeOffset);
        
        builder.Property(userInfo => userInfo.UpdatedDateTimeOffset);

        builder.HasOne(userInfo => userInfo.UserInfoImage)
            .WithOne(userInfoImage => userInfoImage.UserInfo)
            .HasForeignKey<UserInfo>(userInfo => userInfo.UserInfoImageId)
            .IsRequired(false);
    }
}