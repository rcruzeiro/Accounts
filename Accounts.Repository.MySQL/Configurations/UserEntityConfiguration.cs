using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.ID);
            builder.Property(u => u.ClientID).HasColumnName("clientId").IsRequired();
            builder.Property(u => u.Name).HasColumnName("name").IsRequired();
            builder.Property(u => u.Email).HasColumnName("email").IsRequired();
            builder.Property(u => u.Password).HasColumnName("password").IsRequired();
            builder.Property(u => u.LocationID).HasColumnName("locationId");
            builder.Property(u => u.LastLogin).HasColumnName("lastLogin").HasColumnType("datetime");
            builder.Property(u => u.Active).HasColumnName("active");
            builder.Property(u => u.CreatedAt).HasColumnName("createdAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(u => u.UpdatedAt).HasColumnName("updatedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(u => u.RemovedAt).HasColumnName("removedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            //relationships
            builder.HasMany<UserProfile>(u => u.Profiles)
                   .WithOne(up => up.User)
                   .HasForeignKey(up => up.UserID);
            builder.HasMany<UserGrant>(u => u.Grants)
                   .WithOne(up => up.User)
                   .HasForeignKey(up => up.UserID);
            //UK
            builder.HasIndex(u => u.Email).IsUnique();
            //indexes
            builder.HasIndex(u => u.Username);
            builder.HasIndex(u => new { u.Username, u.Password });
            builder.HasIndex(u => u.ClientID);
        }
    }
}
