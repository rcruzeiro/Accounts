using Core.Framework.API.Messages;

namespace Accounts.API.Messages.Profiles
{
    public class CreateProfileRequest : BaseRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
