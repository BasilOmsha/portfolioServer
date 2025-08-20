using System.ComponentModel.DataAnnotations;

namespace Portfolio.Application.DTOs
{
    public class ContactFormDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public required string Message { get; set; }

        [Required(ErrorMessage = "reCAPTCHA token is required.")]
        public required string RecaptchaToken { get; set; }
    }
}