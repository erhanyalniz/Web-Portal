using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorWebApp.Shared;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Only set the BaseAddress if it's not already set
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("https://localhost:7283/");
        }
    }

    public async Task<string> Login(User user)
    {
        var response = await _httpClient.PostAsJsonAsync("user/login", user);

        if (response.IsSuccessStatusCode)
        {
            return "Success";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return "UserNotFound";
        }
        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            return "IncorrectPassword";
        }
        else
        {
            return "Error";
        }
    }

    public async Task<string> SetupTotp(string email)
    {
        TotpSetupRequest request = new TotpSetupRequest();
        request.Email = email;
        var response = await _httpClient.PostAsJsonAsync("user/setup-totp", request);
        if (response.IsSuccessStatusCode)
        {
            var byteArray = await response.Content.ReadAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
        else
        {
            return "Error"; // Handle the error accordingly
        }
    }


    public async Task<string> RegenerateTotp(string email)
    {
        TotpSetupRequest request = new TotpSetupRequest();
        request.Email = email;
        var response = await _httpClient.PostAsJsonAsync("user/regenerate-totp", request);
        if (response.IsSuccessStatusCode)
        {
            var byteArray = await response.Content.ReadAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
        else
        {
            return "Error in regenerating QR Code";
        }
    }

    public async Task<HttpResponseMessage> VerifyTotp(TotpVerificationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("user/verify-totp", request);
        return response;
    }
}
