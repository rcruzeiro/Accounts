namespace Accounts.Entities
{
    public class UserGrant
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int GrantID { get; set; }
        public Grant Grant { get; set; }
    }
}
