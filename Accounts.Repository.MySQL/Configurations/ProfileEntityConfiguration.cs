using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    class ProfileEntityConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("profiles");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ClientID).HasColumnName("clientId").IsRequired();
            builder.Property(p => p.Title).HasColumnName("title").IsRequired();
            builder.Property(p => p.Description).HasColumnName("desc");
            builder.Property(p => p.Active).HasColumnName("active");
            builder.Property(p => p.CreatedAt).HasColumnName("createdAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(p => p.UpdatedAt).HasColumnName("updatedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(p => p.RemovedAt).HasColumnName("removedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            //relationships
            builder.HasMany<ProfileGrant>(p => p.Grants)
                   .WithOne(pp => pp.Profile)
                   .HasForeignKey(pp => pp.ProfileID);
            builder.HasMany<UserProfile>(p => p.Users)
                   .WithOne(up => up.Profile)
                   .HasForeignKey(up => up.ProfileID);
            //indexes
            builder.HasIndex(p => p.ClientID);
        }
    }
}
