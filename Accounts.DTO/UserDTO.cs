using System;
using System.Collections.Generic;

namespace Accounts.DTO
{
    public class UserDTO
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LocationID { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public List<ProfileDTO> Profiles { get; set; } = new List<ProfileDTO>();
        public List<GrantDTO> Grants { get; set; } = new List<GrantDTO>();
        public bool Active { get; set; }
        public string Token { get; set; }
    }
}
