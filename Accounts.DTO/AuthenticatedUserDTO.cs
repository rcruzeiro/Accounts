using System;

namespace Accounts.DTO
{
    public class AuthenticatedUserDTO
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
