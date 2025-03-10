﻿using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        public IConfiguration Configuration { get; } // appsettings.json'ı okumaya yarar (IConfiguration)
        private TokenOptions _tokenOptions; // token'ın değerleri
        private DateTime _accessTokenExpiration; // access token ne zaman geçersizleşecek
        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            // GetSection => JSON'ın içindeki TokenOptions objesini al (appsettings.json'dan)
            // appsettings kısmındakini _topenOptions'a eşliyoruz
            _tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();

        }
        public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);

            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };

        }

        // System.IdentityModel.Tokens.Jwt paketini kullanarak JWT yaratıyoruz
        public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user,
            SigningCredentials signingCredentials, List<OperationClaim> operationClaims)
        {
            var jwt = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        // Claim'ler oluşturulurken, yetkiden daha fazlası
        // Başka bilgiler de oluyor => claiming dediğimiz JWT karşısına gelen baya şey var
        // Kullanıcı Id, Email, Name, Roles set ederiz
        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
        {
            // claims'in içinde AddEmail vs. yok
            // .NET'te var olan sınıflara extension ekleyebiliyoruz (yeni metod)
            // this ICollection<Claim> => Ben bunu extend ediyorum demek
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());

            return claims;
        }
    }
}
