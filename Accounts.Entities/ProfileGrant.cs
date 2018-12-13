namespace Accounts.Entities
{
    public class ProfileGrant
    {
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
        public int GrantID { get; set; }
        public Grant Grant { get; set; }
    }
}
