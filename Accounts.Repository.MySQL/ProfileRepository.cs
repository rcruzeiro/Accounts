using System;
using Accounts.Entities;
using Accounts.Repository.MySQL.Context;

namespace Accounts.Repository.MySQL
{
    public sealed class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(string connstring)
            : base(new ContextFactory(connstring).CreateDbContext())
        { }

        public override void Remove(Profile entity)
        {
            entity.RemovedAt = DateTime.Now;
            base.Remove(entity);
        }
    }
}
