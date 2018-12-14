using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Accounts.Repository.MySQL.Context
{
    class ContextFactory : IDesignTimeDbContextFactory<AccountsContext>
    {
        readonly string _connstring;

        public ContextFactory(string connstring)
        {
            //_connstring = connstring;
            _connstring = "Server=localhost;Port=3306;Uid=root;Pwd=secret;Database=accounts";
        }

        public ContextFactory()
        {
            _connstring =
                "Server=localhost;Port=3306;Uid=root;Pwd=secret;Database=accounts";
        }

        public AccountsContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AccountsContext>();
            builder.UseLazyLoadingProxies();
            builder.UseMySql(_connstring);
            return new AccountsContext(builder.Options);
        }

        public AccountsContext CreateDbContext()
        {
            return CreateDbContext(null);
        }
    }
}
