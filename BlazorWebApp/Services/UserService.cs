using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace BlazorWebApp
{
    public class UserService
    {
        private readonly ILocalStorageService _localStorageService;
        private const string UserEmailKey = "CurrentUserEmail";
        private const string AuthStateKey = "IsUserAuthenticated";

        public UserService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetCurrentUserEmailAsync(string email)
        {
            await _localStorageService.SetItemAsync(UserEmailKey, email);
        }

        public async Task<string> GetCurrentUserEmailAsync()
        {
            var email = await _localStorageService.GetItemAsStringAsync(UserEmailKey);
            // Trim any additional quotes because local storage holds email address like string in string
            return email?.Trim('"'); 
        }
        public async Task SetUserAuthenticatedAsync(bool isAuthenticated)
        {
            await _localStorageService.SetItemAsync(AuthStateKey, isAuthenticated);
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            return await _localStorageService.GetItemAsync<bool>(AuthStateKey);
        }
    }
}
