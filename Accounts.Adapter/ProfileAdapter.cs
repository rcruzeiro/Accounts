using System;
using Accounts.DTO;
using Accounts.Entities;

namespace Accounts.Adapter
{
    public static class ProfileAdapter
    {
        public static Profile Adapt(this ProfileDTO profile)
        {
            try
            {
                if (profile == null)
                    throw new ArgumentNullException(nameof(profile));

                var p = new Profile
                {
                    ID = profile.ID,
                    ClientID = profile.ClientID,
                    Title = profile.Title,
                    Description = profile.Description,
                    Active = profile.Active
                };

                profile.Grants.ForEach(grant =>
                    p.Grants.Add(new ProfileGrant
                    {
                        GrantID = grant.ID
                    }));

                return p;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static ProfileDTO Adapt(this Profile profile)
        {
            try
            {
                if (profile == null)
                    throw new ArgumentNullException(nameof(profile));

                var dto = new ProfileDTO
                {
                    ID = profile.ID,
                    ClientID = profile.ClientID,
                    Title = profile.Title,
                    Description = profile.Description,
                    Active = profile.Active
                };

                profile.Grants.ForEach(grant =>
                    dto.Grants.Add(grant.Grant.Adapt()));

                return dto;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
