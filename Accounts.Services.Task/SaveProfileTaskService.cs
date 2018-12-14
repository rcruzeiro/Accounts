using System;
using System.Threading.Tasks;
using Accounts.Entities;
using Accounts.Services.Entity;

namespace Accounts.Services.Task
{
    public sealed class SaveProfileTaskService
    {
        readonly GrantEntityService _grantEntityService;
        readonly ProfileEntityService _profileEntityService;

        public SaveProfileTaskService(GrantEntityService grantEntityService,
                                        ProfileEntityService profileEntityService)
        {
            _grantEntityService = grantEntityService;
            _profileEntityService = profileEntityService;
        }

        public async Task<bool> Save(Profile profile)
        {
            try
            {
                await _profileEntityService.Save(profile);
                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
