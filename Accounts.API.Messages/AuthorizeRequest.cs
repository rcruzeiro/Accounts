using Core.Framework.API.Messages;

namespace Accounts.API.Messages
{
    public class AuthorizeRequest : BaseRequest
    {
        public string ApplicationID { get; set; }
    }
}
