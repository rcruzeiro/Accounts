using System;
using System.Collections.Generic;

namespace Accounts.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LocationID { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public virtual List<UserProfile> Profiles { get; set; }
        public virtual List<UserGrant> Grants { get; set; }
    }
}
