using Accounts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Accounts.Repository.MySQL.Configurations
{
    sealed class GrantEntityConfiguration : IEntityTypeConfiguration<Grant>
    {
        public void Configure(EntityTypeBuilder<Grant> builder)
        {
            builder.ToTable("permissions");
            builder.HasKey(p => p.ID);
            builder.Property(p => p.ClientID).HasColumnName("clientId").IsRequired();
            builder.Property(p => p.Code).HasColumnName("code").IsRequired();
            builder.Property(p => p.Title).HasColumnName("title").IsRequired();
            builder.Property(p => p.Description).HasColumnName("desc");
            builder.Property(p => p.Action).HasColumnName("action").IsRequired();
            builder.Property(p => p.Active).HasColumnName("active");
            builder.Property(p => p.CreatedAt).HasColumnName("createdAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(p => p.UpdatedAt).HasColumnName("updatedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            builder.Property(p => p.RemovedAt).HasColumnName("removedAt").HasColumnType("datetime").HasDefaultValueSql("now()");
            //relationships
            builder.HasMany<ProfileGrant>(p => p.Profiles)
                   .WithOne(pp => pp.Grant)
                   .HasForeignKey(pp => pp.GrantID);
            builder.HasMany<UserGrant>(p => p.Users)
                   .WithOne(up => up.Grant)
                   .HasForeignKey(up => up.GrantID);
            //UK
            builder.HasIndex(p => p.Code).IsUnique();
            //indexes
            builder.HasIndex(p => p.ClientID);
        }
    }
}
