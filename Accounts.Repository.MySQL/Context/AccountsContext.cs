using Accounts.Repository.MySQL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Repository.MySQL.Context
{
    class AccountsContext : DbContext
    {
        public AccountsContext(DbContextOptions options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProfileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new GrantEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileGrantEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserProfileEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserGrantEntityConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
