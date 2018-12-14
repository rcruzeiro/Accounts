using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    sealed class UserGrantEntityConfiguration : IEntityTypeConfiguration<UserGrant>
    {
        public void Configure(EntityTypeBuilder<UserGrant> builder)
        {
            builder.ToTable("user_grants");
            builder.HasKey(up => new { up.UserID, up.GrantID });
            //relationships
            builder.HasOne<User>(up => up.User)
                   .WithMany(u => u.Grants)
                   .HasForeignKey(up => up.UserID);
            builder.HasOne<Grant>(up => up.Grant)
                   .WithMany(p => p.Users)
                   .HasForeignKey(up => up.GrantID);
        }
    }
}
