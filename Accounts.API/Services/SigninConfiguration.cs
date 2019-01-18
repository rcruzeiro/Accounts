using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Accounts.API.Services
{
    public class SigninConfiguration
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials Credentials { get; set; }

        public SigninConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
                Key = new RsaSecurityKey(provider.ExportParameters(true));

            Credentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
