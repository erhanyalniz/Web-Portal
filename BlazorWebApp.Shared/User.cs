namespace BlazorWebApp.Shared
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

public class TotpVerificationRequest
{
    public string Email { get; set; }
    public string TotpValue { get; set; }
}

public class TotpSetupRequest
{
    public string Email { get; set; }
}