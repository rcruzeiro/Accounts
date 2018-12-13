using System;
using Accounts.Repository;
using Accounts.Repository.MySQL;
using Microsoft.Extensions.Configuration;

namespace Accounts.DI
{
    static class DatabaseFactory
    {
        static readonly string _connname =
            Environment.GetEnvironmentVariable("DB_CONNECTION_NAME");

        public static IGrantRepository GetGrantRepository(IConfiguration configuration)
        {
            return new GrantRepository(configuration.GetConnectionString(_connname));
        }

        public static IProfileRepository GetProfileRepository(IConfiguration configuration)
        {
            return new ProfileRepository(configuration.GetConnectionString(_connname));
        }

        public static IUserRepository GetUserRepository(IConfiguration configuration)
        {
            return new UserRepository(configuration.GetConnectionString(_connname));
        }
    }
}
