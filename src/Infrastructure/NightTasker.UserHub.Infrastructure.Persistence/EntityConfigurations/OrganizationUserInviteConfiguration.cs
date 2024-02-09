using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NightTasker.UserHub.Core.Domain.Entities;

namespace NightTasker.UserHub.Infrastructure.Persistence.EntityConfigurations;

public class OrganizationUserInviteConfiguration : IEntityTypeConfiguration<OrganizationUserInvite>
{
    public void Configure(EntityTypeBuilder<OrganizationUserInvite> builder)
    {
        builder.ToTable("organization_user_invites");
        
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.InviterUser)
            .WithMany()
            .HasForeignKey(x => x.InviterUserId)
            .IsRequired();
        
        builder.HasOne(x => x.InvitedUser)
            .WithMany()
            .HasForeignKey(x => x.InvitedUserId)
            .IsRequired();
        
        builder.HasOne(x => x.Organization)
            .WithMany(organization => organization.OrganizationUserInvites)
            .HasForeignKey(x => x.OrganizationId)
            .IsRequired();
        
        builder.Property(x => x.IsAccepted);
        
        builder.Property(x => x.IsRevoked);
        
        builder.Property(x => x.Message)
            .HasMaxLength(1024);
        
        builder.Property(x => x.CreatedDateTimeOffset);
        
        builder.Property(x => x.UpdatedDateTimeOffset);
    }
}