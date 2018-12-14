using System.Collections.Generic;

namespace Accounts.Entities
{
    public class Grant : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActionType Action { get; set; }
        public virtual List<ProfileGrant> Profiles { get; set; }
        public virtual List<UserGrant> Users { get; set; }

        public Grant()
        {
            Profiles = new List<ProfileGrant>();
            Users = new List<UserGrant>();
        }
    }
}
