using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    sealed class ProfileGrantEntityConfiguration : IEntityTypeConfiguration<ProfileGrant>
    {
        public void Configure(EntityTypeBuilder<ProfileGrant> builder)
        {
            builder.ToTable("profile_grants");
            builder.HasKey(pp => new { pp.ProfileID, pp.GrantID });
            //relationships
            builder.HasOne<Profile>(pp => pp.Profile)
                   .WithMany(p => p.Grants)
                   .HasForeignKey(pp => pp.ProfileID);
            builder.HasOne<Grant>(pp => pp.Grant)
                   .WithMany(p => p.Profiles)
                   .HasForeignKey(pp => pp.GrantID);
        }
    }
}
