using Microsoft.AspNetCore.Mvc;
using BlazorWebApp.Shared;
using System.Data.SqlClient;
using QRCoder;
using System.Drawing;
using System.IO;
using OtpNet; 

namespace BlazorWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string connectionString = "Server=ERHANASUS;Database=ClientPortal;User ID=sa;Password=e-Lips2009;";

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            var validationResult = ValidateUser(user.Email, user.Password);
            if (validationResult == UserValidationResult.Success)
            {
                return Ok();
            }
            else if (validationResult == UserValidationResult.UserNotFound)
            {
                return NotFound("User not found.");
            }
            else
            {
                return Unauthorized("Incorrect password.");
            }
        }

        [HttpPost("setup-totp")]
        public IActionResult SetupTotp([FromBody] TotpSetupRequest emailFromRequest)
        {
            var existingSecret = GetTotpSecret(emailFromRequest.Email);
            if (!string.IsNullOrEmpty(existingSecret))
            {
                return Ok(new { qrCode = "NoQRCodeNeeded" });
            }

            var key = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(key);
            SetTotpSecret(emailFromRequest.Email, base32Secret);

            string issuer = "ErnieWebPortal";
            string totpUrl = $"otpauth://totp/{issuer}:{emailFromRequest.Email}?secret={base32Secret}&issuer={issuer}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = pngByteQRCode.GetGraphic(20);

            return File(qrCodeBytes, "image/png");
        }


        [HttpPost("verify-totp")]
        public IActionResult VerifyTotp([FromBody] TotpVerificationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.TotpValue))
            {
                return BadRequest("Verification code value is missing.");
            }

            var secretKey = GetTotpSecret(request.Email);
            if (string.IsNullOrEmpty(secretKey))
            {
                return NotFound("TOTP not set up for this user.");
            }

            var totp = new Totp(Base32Encoding.ToBytes(secretKey));
            if (totp.VerifyTotp(request.TotpValue, out long timeStepMatched, new VerificationWindow(2, 2)))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Invalid verification code. Please try again or regenerate authentication.");
            }
        }


        [HttpPost("regenerate-totp")]
        public IActionResult RegenerateTotp([FromBody] TotpSetupRequest emailFromRequest)
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            var base32Secret = Base32Encoding.ToString(key);
            SetTotpSecret(emailFromRequest.Email, base32Secret);

            string issuer = "ErnieWebPortal";
            string totpUrl = $"otpauth://totp/{issuer}:{emailFromRequest.Email}?secret={base32Secret}&issuer={issuer}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(totpUrl, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQRCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = pngByteQRCode.GetGraphic(20);

            return File(qrCodeBytes, "image/png");
        }

        private UserValidationResult ValidateUser(string email, string password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SELECT Password FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                connection.Open();
                var result = command.ExecuteScalar() as string;

                if (result == null)
                {
                    return UserValidationResult.UserNotFound;
                }
                else if (result != password) // Replace this with password hash comparison in a real app
                {
                    return UserValidationResult.IncorrectPassword;
                }

                return UserValidationResult.Success;
            }
        }

        private void SetTotpSecret(string email, string secret)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("UPDATE Users SET Token = @Secret WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                if (secret == null)
                {
                    command.Parameters.AddWithValue("@Secret", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@Secret", secret);
                }
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private string GetTotpSecret(string email)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SELECT Token FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();
                return command.ExecuteScalar() as string;
            }
        }

        public enum UserValidationResult
        {
            Success,
            UserNotFound,
            IncorrectPassword
        }
    }
}
