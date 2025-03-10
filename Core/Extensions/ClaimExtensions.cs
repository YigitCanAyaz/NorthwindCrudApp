﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Core.Extensions
{
    // Extension sınıfları static olur
    public static class ClaimExtensions
    {
        // this ICollection<Claim> => Ben bunu extend ediyorum demek
        // Burada var olan class'ı extend edip yeni metodlar yazıyoruz
        // ICollection türünde Claim'i extend ediyoruz (this => neyi extend edeceğiz, ve parametre (email))
        public static void AddEmail(this ICollection<Claim> claims, string email)
        {
            // Çeşitli kaydedilmiş isimler var (JwtRegisteredClaimNames'in içerisinde) onlara email ekliyoruz
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));
        }

        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }

        public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
        }

        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            // her bir rolü tek tek claim'e ekle
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
    }
}
