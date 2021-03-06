﻿using System;
using Accounts.Entities;
using Accounts.Repository.MySQL.Context;
using Microsoft.EntityFrameworkCore;

namespace Accounts.Repository.MySQL
{
    public sealed class ProfileRepository : BaseRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(string connstring)
            : base(new ContextFactory(connstring).CreateDbContext())
        { }

        public override void Update(Profile entity)
        {
            entity.Grants.ForEach(grant =>
                _context.Entry(grant.Grant).State = EntityState.Unchanged);
            base.Update(entity);
        }

        public override void Remove(Profile entity)
        {
            entity.RemovedAt = DateTime.Now;
            base.Remove(entity);
        }
    }
}
