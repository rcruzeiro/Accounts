using System;
using Accounts.DTO;
using Accounts.Entities;

namespace Accounts.Adapter
{
    public static class UserAdapter
    {
        public static User Adapt(this UserDTO user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var u = new User
                {
                    ID = user.ID,
                    ClientID = user.ClientID,
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    LastLogin = user.LastLogin,
                    Active = user.Active
                };

                user.Profiles.ForEach(profile =>
                    u.Profiles.Add(new UserProfile
                    {
                        ProfileID = profile.ID,
                        Profile = profile.Adapt()
                    }));

                user.Grants.ForEach(grant =>
                    u.Grants.Add(new UserGrant
                    {
                        GrantID = grant.ID,
                        Grant = grant.Adapt()
                    }));

                return u;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static UserDTO Adapt(this User user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                var dto = new UserDTO
                {
                    ID = user.ID,
                    ClientID = user.ClientID,
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    LastLogin = user.LastLogin,
                    Active = user.Active
                };

                user.Profiles.ForEach(profile =>
                    dto.Profiles.Add(profile.Profile.Adapt()));

                user.Grants.ForEach(grant =>
                    dto.Grants.Add(grant.Grant.Adapt()));

                return dto;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
