﻿using System.Collections.Generic;

namespace Accounts.Entities
{
    public class Profile : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual List<ProfileGrant> Grants { get; set; } = new List<ProfileGrant>();
        public virtual List<UserProfile> Users { get; set; } = new List<UserProfile>();
    }
}
