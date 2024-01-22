using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class OrganizationEntityConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(organization => organization.Id);
        
        builder.Property(organization => organization.Name)
            .HasMaxLength(254);

        builder.Property(organization => organization.CreatedDateTimeOffset);
        
        builder.Property(organization => organization.UpdatedDateTimeOffset);
    }
}