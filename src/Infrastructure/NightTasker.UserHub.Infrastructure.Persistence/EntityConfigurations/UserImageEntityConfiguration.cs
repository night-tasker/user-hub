using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class UserImageEntityConfiguration : IEntityTypeConfiguration<UserImage>
{
    public void Configure(EntityTypeBuilder<UserImage> builder)
    {
        builder.ToTable(name: "user_images");
        
        builder.HasKey(userImage => userImage.Id);

        builder.Property(userImage => userImage.IsActive);
        
        builder.Property(userImage => userImage.FileName)
            .HasMaxLength(254);
        
        builder.Property(userImage => userImage.Extension)
            .HasMaxLength(32);
        
        builder.Property(userImage => userImage.ContentType)
            .HasMaxLength(64);
        
        builder.Property(userImage => userImage.FileSize);
        
        builder.Property(userImage => userImage.CreatedDateTimeOffset);
        
        builder.Property(userImage => userImage.UpdatedDateTimeOffset);
        
        builder.HasOne(userImage => userImage.UserInfo)
            .WithMany(user => user.UserInfoImages)
            .HasForeignKey(userImage => userImage.UserInfoId)
            .IsRequired();
    }
}