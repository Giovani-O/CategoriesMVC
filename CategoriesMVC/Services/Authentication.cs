using CategoriesMVC.Models;
using System.Text.Json;
using System.Text;

namespace CategoriesMVC.Services
{
    public class Authentication : IAuthentication
    {
        private readonly IHttpClientFactory _clientFactory;
        const string apiAuthEndpoint = "/api/Auth/login?api-version=1";
        private readonly JsonSerializerOptions _options;
        private TokenViewModel userToken;
        public Authentication(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<TokenViewModel> AuthenticateUser(UserViewModel userViewModel)
        {
            var client = _clientFactory.CreateClient("AuthenticationApi");
            var user = JsonSerializer.Serialize(userViewModel);
            StringContent content = new StringContent(user, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiAuthEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    userToken = await JsonSerializer
                                  .DeserializeAsync<TokenViewModel>
                                  (apiResponse, _options);
                }
                else
                {
                    return null;
                }
            }
            return userToken;
        }
    }
}
