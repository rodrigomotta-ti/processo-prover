using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Prover.Entrevista.Core.Application.Interfaces;
using Prover.Entrevista.Core.Domain.Entities;
using Prover.Entrevista.Core.Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Prover.Entrevista.Core.Application.Services.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtAuthSettings;

    public JwtTokenService(IOptions<JwtSettings> JwtAuthOptions)
    {
        _jwtAuthSettings = JwtAuthOptions.Value;
    }

    public string GenerateAuthToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtAuthSettings.Issuer,
            audience: _jwtAuthSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtAuthSettings.ExpiryInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}