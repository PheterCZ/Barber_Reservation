using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Data;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSecurityTokenHandler _tokenHandler = new();

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string> GenerateTokenAsync(ApplicationUser user, IList<string> roles)
        {
            var key = GetRequiredJwtSetting("Jwt:Key");
            var issuer = GetRequiredJwtSetting("Jwt:Issuer");
            var audience = GetRequiredJwtSetting("Jwt:Audience");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
            );

            return Task.FromResult(_tokenHandler.WriteToken(token));
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            var key = GetRequiredJwtSetting("Jwt:Key");
            var issuer = GetRequiredJwtSetting("Jwt:Issuer");
            var audience = GetRequiredJwtSetting("Jwt:Audience");

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);
                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch (SecurityTokenException)
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }

        private string GetRequiredJwtSetting(string key)
        {
            return _configuration[key]
                ?? throw new InvalidOperationException($"Missing JWT configuration key: {key}");
        }
    }
}