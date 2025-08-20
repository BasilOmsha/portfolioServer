using System.Text.Json.Serialization;

namespace Portfolio.Application.DTOs
{
    public class RecaptchaResponseDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("challenge_ts")]
        public string? ChallengeTs { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        [JsonPropertyName("error-codes")]
        public string[]? ErrorCodes { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; } // For reCAPTCHA v3

        [JsonPropertyName("action")]
        public string? Action { get; set; } // For reCAPTCHA v3
    }
}