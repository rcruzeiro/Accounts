using Accounts.Entities;
using Accounts.Repository.MySQL.Context;

namespace Accounts.Repository.MySQL
{
    public sealed class GrantRepository : BaseRepository<Grant>, IGrantRepository
    {
        public GrantRepository(string connstring)
            : base(new ContextFactory(connstring).CreateDbContext())
        { }
    }
}
