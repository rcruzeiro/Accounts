using Core.Framework.API.Messages;

namespace Accounts.API.Messages.User
{
    public class AuthenticateUserRequest : BaseRequest
    {
        public string ApplicationID { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
