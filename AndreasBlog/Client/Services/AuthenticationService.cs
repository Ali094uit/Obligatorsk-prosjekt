using System;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Blazored.LocalStorage;

namespace AndreasBlog.Client.Services
{
    public class AuthenticationService
    {

        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<bool> IsUserAuthorized()
        {
            var token = await _localStorage.GetItemAsync<string>("jwtToken");

            if (!string.IsNullOrEmpty(token))
            {
                // Dekode tokenet
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    // Få ut claims
                    var claims = jsonToken.Claims.ToList();

                    var userIdClaim = claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;
                    var roleClaim = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

                    if (!string.IsNullOrEmpty(userIdClaim) && roleClaim == "bruker")
                    {
                        return true;
                    }
                }
            }

            Console.WriteLine("Token is invalid or does not contain required claims.");
            return false;
        }

        public async Task<bool> Logout()
        {
            try
            {
                // Fjern JWT-tokenet fra local storage
                await _localStorage.RemoveItemAsync("jwtToken");

                return true; // Logg ut vellykket
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Feil under logg ut: {ex.Message}");
                return false; // Logg ut mislyktes
            }
        }


    }

}

