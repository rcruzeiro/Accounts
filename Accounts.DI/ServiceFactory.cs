using Accounts.Services.Entity;
using Microsoft.Extensions.Configuration;

namespace Accounts.DI
{
    static class ServiceFactory
    {
        public static GrantEntityService GetGrantEntityService(IConfiguration configuration)
        {
            return new GrantEntityService(DatabaseFactory.GetGrantRepository(configuration));
        }

        public static ProfileEntityService GetProfileEntityService(IConfiguration configuration)
        {
            return new ProfileEntityService(DatabaseFactory.GetProfileRepository(configuration));
        }

        public static UserEntityService GetUserEntityService(IConfiguration configuration)
        {
            return new UserEntityService(DatabaseFactory.GetUserRepository(configuration));
        }
    }
}
