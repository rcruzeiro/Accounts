namespace Accounts.Entities
{
    public class UserProfile
    {
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public int ProfileID { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
