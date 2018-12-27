using Core.Framework.API.Messages;

namespace Accounts.API.Messages.User
{
    public class ValidateAuthRequest : BaseRequest
    {
        public string ApplicationID { get; set; }
        public string AccessToken { get; set; }
    }
}
