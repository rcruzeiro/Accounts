using System;

namespace Accounts.DTO
{
    public class AuthorizeDTO
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
