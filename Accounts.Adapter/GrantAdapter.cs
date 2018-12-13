using System;
using Accounts.DTO;
using Accounts.Entities;

namespace Accounts.Adapter
{
    public static class GrantAdapter
    {
        public static Grant Adapt(this GrantDTO permission)
        {
            try
            {
                if (permission == null)
                    throw new ArgumentNullException(nameof(permission));

                return new Grant
                {
                    ID = permission.ID,
                    ClientID = permission.ClientID,
                    Code = permission.Code,
                    Title = permission.Title,
                    Description = permission.Description,
                    Action = (ActionType)permission.Action,
                    Active = permission.Active
                };
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static GrantDTO Adapt(this Grant permission)
        {
            try
            {
                if (permission == null)
                    throw new ArgumentNullException(nameof(permission));

                return new GrantDTO
                {
                    ID = permission.ID,
                    ClientID = permission.ClientID,
                    Code = permission.Code,
                    Title = permission.Title,
                    Description = permission.Description,
                    Action = (int)permission.Action,
                    Active = permission.Active
                };
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
