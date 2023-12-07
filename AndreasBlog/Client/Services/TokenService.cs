using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;


namespace AndreasBlog.Client.Services
{
    public class TokenService
    {
        private readonly ILocalStorageService _localStorage;

        public TokenService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("jwtToken");
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetItemAsync("jwtToken", token);
        }

        public async Task RemoveTokenAsync()
        {
            await _localStorage.RemoveItemAsync("jwtToken");
        }

        public async Task<string?> GetUserIdAsync()
        {
            var jwtToken = await GetTokenAsync();

            if (!string.IsNullOrEmpty(jwtToken))
            {
                // Hent brukerens ID fra JWT-tokenet
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;
                var userId = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

                return userId;
            }

            return null;
        }
    }
}

