using System.ComponentModel.DataAnnotations;

namespace Portfolio.Infrastructure.Configuration
{
    public class EmailSettings
    {
        [Required]
        public required string SmtpServer { get; set; }

        [Required]
        public int SmtpPort { get; set; }

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string FromEmail { get; set; }
    }
}