using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Accounts.Entities;
using Accounts.Services.Entity;

namespace Accounts.Services.Task
{
    public sealed class SaveProfileGrantsTaskService
    {
        readonly GrantEntityService _grantEntityService;
        readonly ProfileEntityService _profileEntityService;

        public SaveProfileGrantsTaskService(GrantEntityService grantEntityService, ProfileEntityService profileEntityService)
        {
            _grantEntityService = grantEntityService;
            _profileEntityService = profileEntityService;
        }

        public async Task<bool> Save(string clientID, int profileID, List<int> grantIDs)
        {
            try
            {
                var profile = _profileEntityService.GetProfile(clientID, profileID);
                profile.Grants.Clear();
                grantIDs.ForEach(id =>
                {
                    var grant = _grantEntityService.GetGrant(profile.ClientID, id);
                    profile.Grants.Add(new ProfileGrant
                    {
                        Grant = grant
                    });
                });
                await _profileEntityService.Save(profile);
                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
