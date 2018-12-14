using System;
using Accounts.Repository;
using Accounts.Repository.MySQL;
using Microsoft.Extensions.Configuration;

namespace Accounts.DI
{
    static class DatabaseFactory
    {
        static readonly string _conname =
            Environment.GetEnvironmentVariable("DB_CONNECTION_NAME");

        public static IGrantRepository GetGrantRepository(IConfiguration configuration)
        {
            return new GrantRepository(configuration.GetConnectionString(_conname));
        }

        public static IProfileRepository GetProfileRepository(IConfiguration configuration)
        {
            return new ProfileRepository(configuration.GetConnectionString(_conname));
        }

        public static IUserRepository GetUserRepository(IConfiguration configuration)
        {
            return new UserRepository(configuration.GetConnectionString(_conname));
        }
    }
}
