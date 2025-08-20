using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Portfolio.Application.Interfaces;
using Portfolio.Application.DTOs;
using Portfolio.Infrastructure.Configuration;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Services
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly RecaptchaSettings _recaptchaSettings;

        public RecaptchaService(HttpClient httpClient, IOptions<RecaptchaSettings> recaptchaSettings)
        {
            _httpClient = httpClient;
            _recaptchaSettings = recaptchaSettings.Value;
        }

        public async Task<Result> ValidateRecaptcha(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return Result.Failure("reCAPTCHA token is required.");
                }

                // Create form data for the POST request (more secure than query string)
                var formData = new List<KeyValuePair<string, string>>
                {
                    new("secret", _recaptchaSettings.SecretKey),
                    new("response", token)
                };

                var content = new FormUrlEncodedContent(formData);
                var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    return Result.Failure("Failed to validate reCAPTCHA with Google's servers.");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var recaptchaResponse = JsonSerializer.Deserialize<RecaptchaResponseDto>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (recaptchaResponse == null)
                {
                    return Result.Failure("Invalid response from reCAPTCHA service.");
                }

                if (!recaptchaResponse.Success)
                {
                    var errorMessage = "reCAPTCHA verification failed.";
                    if (recaptchaResponse.ErrorCodes?.Length > 0)
                    {
                        errorMessage += $" Errors: {string.Join(", ", recaptchaResponse.ErrorCodes)}";
                    }
                    return Result.Failure(errorMessage);
                }

                // Optional: Add additional validation
                // - Check hostname matches your domain
                // - Check challenge timestamp is recent
                // - Check score for v3 (if using reCAPTCHA v3)

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"reCAPTCHA validation failed: {ex.Message}");
            }
        }
    }
}