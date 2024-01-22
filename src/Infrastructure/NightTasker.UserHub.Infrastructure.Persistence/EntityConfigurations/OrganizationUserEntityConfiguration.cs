using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class OrganizationUserEntityConfiguration : IEntityTypeConfiguration<OrganizationUser>
{
    public void Configure(EntityTypeBuilder<OrganizationUser> builder)
    {
        builder.HasKey(organizationUser => new { organizationUser.OrganizationId, organizationUser.UserId });
        
        builder.HasOne(organizationUser => organizationUser.Organization)
            .WithMany(organization => organization.OrganizationUsers)
            .HasForeignKey(organizationUser => organizationUser.OrganizationId);
        
        builder.HasOne(organizationUser => organizationUser.UserInfo)
            .WithMany(userInfo => userInfo.OrganizationUsers)
            .HasForeignKey(organizationUser => organizationUser.UserId);
    }
}