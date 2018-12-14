namespace Accounts.Entities
{
    public class ProfileGrant
    {
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }
        public int GrantID { get; set; }
        public virtual Grant Grant { get; set; }
    }
}
