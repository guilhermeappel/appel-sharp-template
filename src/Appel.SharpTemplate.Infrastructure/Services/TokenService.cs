using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Domain.Entities;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Appel.SharpTemplate.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly JwtTokenSettings _jtwTokenSettings;

    public TokenService(IOptions<JwtTokenSettings> optionsJtwTokenSettings)
    {
        _jtwTokenSettings = optionsJtwTokenSettings.Value;
    }

    public string GenerateToken(UserEntity userEntity)
    {
        var symmetricSecutiryKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jtwTokenSettings.SecretKey!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, userEntity.Email!),
                new Claim(JwtRegisteredClaimNames.FamilyName, userEntity.Surname!),
                new Claim(JwtRegisteredClaimNames.GivenName, userEntity.Name!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userEntity.ExternalId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jtwTokenSettings.ExpiryMinutes),
            SigningCredentials =
                new SigningCredentials(symmetricSecutiryKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
