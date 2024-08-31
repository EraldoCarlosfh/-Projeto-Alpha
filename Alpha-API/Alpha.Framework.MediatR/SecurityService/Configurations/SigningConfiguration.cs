using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Alpha.Framework.MediatR.SecurityService.Configurations
{
    public class SigningConfiguration
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfiguration(string secretKey)
        {
            var secretBytes = Encoding.ASCII.GetBytes(secretKey);
            Key = new SymmetricSecurityKey(secretBytes);
            SigningCredentials =
                new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
