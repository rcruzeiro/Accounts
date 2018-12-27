using System;
using Accounts.DTO;
using Accounts.Entities;

namespace Accounts.Adapter
{
    public static class GrantAdapter
    {
        public static Grant Adapt(this GrantDTO grant)
        {
            try
            {
                if (grant == null)
                    throw new ArgumentNullException(nameof(grant));

                return new Grant
                {
                    ID = grant.ID,
                    ClientID = grant.ClientID,
                    Code = grant.Code,
                    Title = grant.Title,
                    Description = grant.Description,
                    Action = (ActionType)grant.Action,
                    Active = grant.Active
                };
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static GrantDTO Adapt(this Grant grant)
        {
            try
            {
                if (grant == null)
                    throw new ArgumentNullException(nameof(grant));

                return new GrantDTO
                {
                    ID = grant.ID,
                    ClientID = grant.ClientID,
                    Code = grant.Code,
                    Title = grant.Title,
                    Description = grant.Description,
                    Action = (int)grant.Action,
                    Active = grant.Active
                };
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
