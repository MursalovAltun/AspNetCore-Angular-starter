using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Autofac.Extras.RegistrationAttributes.RegistrationAttributes;
using EF.Models.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Auth
{
    [As(typeof(IAuthenticationResultProvider))]
    public class AuthenticationResultProvider : IAuthenticationResultProvider
    {
        private readonly JwtTokenOptions _jwtTokenOptions;

        public AuthenticationResultProvider(IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _jwtTokenOptions = jwtTokenOptions.Value;
        }

        public AuthenticateResult Get(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(_jwtTokenOptions.ExpireDays),
                SigningCredentials = new SigningCredentials(_jwtTokenOptions.SymmetricSecurityKey,
                    SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticateResult
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}