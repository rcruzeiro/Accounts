using System.Collections.Generic;
using Core.Framework.API.Messages;

namespace Accounts.API.Messages.Profiles
{
    public class CreateProfileRequest : BaseRequest
    {
        public string ClientID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<int> GrantIDs { get; set; }
    }
}
