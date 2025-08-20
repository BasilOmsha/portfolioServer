using System;

namespace Portfolio.Domain.Entities
{
    public class ContactForm
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Message { get; set; }
        public string? RecaptchaToken { get; set; }
        public DateTime SubmittedAt { get; set; }

        public ContactForm()
        {
            Id = Guid.NewGuid();
            SubmittedAt = DateTime.UtcNow;
        }
    }
}