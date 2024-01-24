using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class OrganizationUserEntityConfiguration : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.ToTable(name: "organization_users");
        
        builder.HasKey(organizationUser => new { organizationUser.OrganizationId, organizationUser.UserId });

        builder.Property(organizationUser => organizationUser.Role)
            .HasConversion<string>()
            .HasDefaultValue(OrganizationUserRole.Member);
        
        builder.HasIndex(organizationUser => organizationUser.Role);
            
        builder.HasOne(organizationUser => organizationUser.Organization)
            .WithMany(organization => organization.OrganizationUsers)
            .HasForeignKey(organizationUser => organizationUser.OrganizationId);
        
        builder.HasOne(organizationUser => organizationUser.UserInfo)
            .WithMany(userInfo => userInfo.OrganizationUsers)
            .HasForeignKey(organizationUser => organizationUser.UserId);
    }
}