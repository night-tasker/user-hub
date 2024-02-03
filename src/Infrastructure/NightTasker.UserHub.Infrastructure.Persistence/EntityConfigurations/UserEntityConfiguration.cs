using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(name: "users");
        
        builder.HasKey(user => user.Id);

        builder.Property(user => user.UserName)
            .HasMaxLength(32);

        builder.Property(user => user.FirstName)
            .HasMaxLength(32);
        
        builder.Property(user => user.MiddleName)
            .HasMaxLength(32);
        
        builder.Property(user => user.LastName)
            .HasMaxLength(32);
        
        builder.Property(user => user.Email)
            .HasMaxLength(254);

        builder.Property(user => user.CreatedDateTimeOffset);
        
        builder.Property(user => user.UpdatedDateTimeOffset);

        builder.HasMany(user => user.UserImages)
            .WithOne(userImage => userImage.User)
            .HasForeignKey(userImage => userImage.UserId)
            .IsRequired(false);
    }
}