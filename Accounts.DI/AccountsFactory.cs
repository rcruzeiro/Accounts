using Accounts.Services.Entity;
using Microsoft.Extensions.Configuration;

namespace Accounts.DI
{
    public class AccountsFactory
    {
        static AccountsFactory _instance;

        public static AccountsFactory Instance
        { get { return _instance ?? (_instance = new AccountsFactory()); } }

        AccountsFactory()
        { }

        public GrantEntityService GetGrant(IConfiguration configuration)
        {
            return ServiceFactory.GetGrantEntityService(configuration);
        }

        public ProfileEntityService GetProfile(IConfiguration configuration)
        {
            return ServiceFactory.GetProfileEntityService(configuration);
        }

        public UserEntityService GetUser(IConfiguration configuration)
        {
            return ServiceFactory.GetUserEntityService(configuration);
        }
    }
}
