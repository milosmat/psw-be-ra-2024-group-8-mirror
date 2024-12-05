using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class TokenService
    {
        private string _secretKey = "this_is_a_very_long_and_secure_key_that_is_at_least_256_bits_in_length_";  // Sigurni ključ sa 256 bita

        public string GenerateTourPurchaseToken(long touristId, long tourId)
        {
            // Definiši claim-ove za JWT
            var claims = new List<Claim>
        {
            new Claim("touristId", touristId.ToString()),
            new Claim("tourId", tourId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            // Generiši JWT token
            var token = CreateToken(claims, 60 * 24); // 24h važenje tokena

            return token;
        }

        private string CreateToken(IEnumerable<Claim> claims, double expirationTimeInMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "explorer",
                audience: "explorer-front.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(expirationTimeInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
