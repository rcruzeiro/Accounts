namespace Accounts.DTO
{
    public class GrantDTO
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Action { get; set; }
        public bool Active { get; set; }
    }
}
