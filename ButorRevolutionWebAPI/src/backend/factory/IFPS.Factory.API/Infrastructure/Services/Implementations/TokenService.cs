using IFPS.Factory.API.Infrastructure.Services.Interfaces;
using IFPS.Factory.Application.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IFPS.Factory.API.Infrastructure.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public TokenDto GenerateToken(AccountDto accountDto)
        {
            if (accountDto == default(AccountDto))
            {
                return new TokenDto();
            }

            return new TokenDto
            {
                AccessToken = GenerateAccessToken(accountDto),
                RefreshToken = GenerateRefreshToken()
            };
        }

        public TokenDto Refresh(TokenDto oldTokenDto)
        {
            return new TokenDto
            {
                AccessToken = GenerateAccessToken(new AccountDto()),
                RefreshToken = GenerateRefreshToken()
            };
        }

        private string GenerateAccessToken(AccountDto accountDto)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningSecret"]));

            var claims = new List<Claim>
            {
                new Claim("UserId", accountDto.Id.ToString()),
                new Claim("CompanyId", accountDto.CompanyId.ToString()),
                new Claim("Email", accountDto.Email),
                new Claim("UserName", accountDto.UserName),
                new Claim("RoleName", accountDto.RoleName),
                new Claim("Language", accountDto.Language.ToString()),
                new Claim("ImageContainerName", accountDto.Image.ContainerName),
                new Claim("ImageFileName", accountDto.Image.FileName)
            };
            claims.AddRange(accountDto.Claims);

            var jwtToken = new JwtSecurityToken(issuer: "ENCO",
                audience: "Anyone",
                claims: claims,
                notBefore: Clock.Now,
                expires: Clock.Now.AddMinutes(int.Parse(configuration["Jwt:ExpiryDuration"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GetNameFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningSecret"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            //return principal.Claims.Single(ent => ent.Type == "UserName").Value; 
            return principal.Claims.Single(ent => ent.Type == "Email").Value; 
        }
    }
}