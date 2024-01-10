using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

/// <summary>
/// Конфигурация сущности <see cref="UserInfoImage"/>
/// </summary>
public class UserInfoImageEntityConfiguration : IEntityTypeConfiguration<UserInfoImage>
{
    public void Configure(EntityTypeBuilder<UserInfoImage> builder)
    {
        builder.ToTable(name: "user_info_images");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Extension)
            .HasMaxLength(32);
        
        builder.Property(x => x.CreatedDateTimeOffset);

        builder.Property(x => x.UpdatedDateTimeOffset);
    }
}