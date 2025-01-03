using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using Octarine_Core.Models;

namespace Octarine_Core.Apis
{
    internal class JWT
    {
        public static EnteredUserLite GetUserNameFromToken(string Jsontoken)
        {
            var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(Jsontoken);
            string token = responseData["token"];
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(token))
            {
                throw new ArgumentException("Неверный JWT токен");
            }
            var jwtToken = jwtHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            var userName = claims.FirstOrDefault(c => c.Type == "users_name")?.Value;
            var userId = claims.FirstOrDefault(i => i.Type == "users_id")?.Value;
            EnteredUserLite es = new EnteredUserLite(Convert.ToInt32(userId), userName.ToString());
            return es;
        }
    }
}
