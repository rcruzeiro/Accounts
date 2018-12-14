using System;
using Accounts.Entities;
using Accounts.Repository.MySQL.Context;

namespace Accounts.Repository.MySQL
{
    public sealed class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(string connstring)
            : base(new ContextFactory(connstring).CreateDbContext())
        { }

        public override void Remove(User entity)
        {
            entity.RemovedAt = DateTime.Now;
            base.Remove(entity);
        }
    }
}
