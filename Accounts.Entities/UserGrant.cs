namespace Accounts.Entities
{
    public class UserGrant
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int GrantID { get; set; }
        public virtual Grant Grant { get; set; }
    }
}
