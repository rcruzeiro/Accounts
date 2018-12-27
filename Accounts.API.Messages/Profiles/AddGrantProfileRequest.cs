using System.Collections.Generic;
using Core.Framework.API.Messages;

namespace Accounts.API.Messages.Profiles
{
    public class AddGrantProfileRequest : BaseRequest
    {
        public List<int> GrantIDs { get; set; }
    }
}
