using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

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

        builder.HasMany(userInfo => userInfo.UserInfoImages)
            .WithOne(userInfoImage => userInfoImage.UserInfo)
            .HasForeignKey(userInfoImage => userInfoImage.UserInfoId)
            .IsRequired(false);
    }
}