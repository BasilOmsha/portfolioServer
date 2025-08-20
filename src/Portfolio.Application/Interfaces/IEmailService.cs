using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Interfaces
{
    public interface IEmailService
    {
        Task<Result> SendContactEmail(ContactForm contactForm);
        Task<Result> SendEmailAsync(string to, string subject, string body);
    }
}