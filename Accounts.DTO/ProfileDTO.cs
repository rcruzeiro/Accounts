using System.Collections.Generic;

namespace Accounts.DTO
{
    public class ProfileDTO
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<GrantDTO> Grants { get; set; } = new List<GrantDTO>();
        public bool Active { get; set; }
    }
}
