using System.ComponentModel.DataAnnotations;

namespace Portfolio.Infrastructure.Configuration
{
    public class RecaptchaSettings
    {
        [Required]
        public required string SiteKey { get; set; }

        [Required]
        public required string SecretKey { get; set; }
    }
}