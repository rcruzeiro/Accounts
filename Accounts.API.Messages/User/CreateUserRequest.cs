using Core.Framework.API.Messages;

namespace Accounts.API.Messages.User
{
    public class CreateUserRequest : BaseRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LocationID { get; set; }
    }
}
