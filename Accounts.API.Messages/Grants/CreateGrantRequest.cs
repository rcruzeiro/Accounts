using Core.Framework.API.Messages;

namespace Accounts.API.Messages.Grants
{
    public class CreateGrantRequest : BaseRequest
    {
        public string ClientID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Action { get; set; }
    }
}
