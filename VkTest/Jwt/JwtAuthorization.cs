using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VkTest.Jwt;

public class JwtAuthorization
{
    private readonly string? key;
    
    public JwtAuthorization(string key)
    {
        this.key = key;
    }
    
    public string? Authenticate(string? username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenkey = Encoding.ASCII.GetBytes(key);
        var tokenDiscriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenkey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDiscriptor);
        return tokenHandler.WriteToken(token);
    }
}