namespace Accounts.DTO
{
    public class TokenDTO
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }
}
