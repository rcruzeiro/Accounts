using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    sealed class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("user_profiles");
            builder.HasKey(up => new { up.UserID, up.ProfileID });
            //relationships
            builder.HasOne<User>(up => up.User)
                   .WithMany(u => u.Profiles)
                   .HasForeignKey(up => up.UserID);
            builder.HasOne<Profile>(up => up.Profile)
                   .WithMany(p => p.Users)
                   .HasForeignKey(up => up.ProfileID);
        }
    }
}
