using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Portfolio.Application.Interfaces;
using Portfolio.Infrastructure.Configuration;
using Portfolio.Domain.Common;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly EmailJsSettings _emailJsSettings;

        public EmailService(HttpClient httpClient, IOptions<EmailJsSettings> emailJsSettings)
        {
            _httpClient = httpClient;
            _emailJsSettings = emailJsSettings.Value;

            // Validate required configuration
            if (string.IsNullOrEmpty(_emailJsSettings.ServiceId))
                throw new InvalidOperationException("EmailJS ServiceId is required but not configured");
            if (string.IsNullOrEmpty(_emailJsSettings.TemplateId))
                throw new InvalidOperationException("EmailJS TemplateId is required but not configured");
            if (string.IsNullOrEmpty(_emailJsSettings.PublicKey))
                throw new InvalidOperationException("EmailJS PublicKey is required but not configured");
            if (string.IsNullOrEmpty(_emailJsSettings.PrivateKey))
                throw new InvalidOperationException("EmailJS PrivateKey is required but not configured");
        }

        public async Task<Result> SendContactEmail(ContactForm contactForm)
        {
            try
            {
                var emailData = new
                {
                    service_id = _emailJsSettings.ServiceId?.Trim(),
                    template_id = _emailJsSettings.TemplateId?.Trim(),
                    user_id = _emailJsSettings.PublicKey?.Trim(),
                    accessToken = _emailJsSettings.PrivateKey?.Trim(),
                    template_params = new
                    {
                        from_name = contactForm.Name,
                        from_email = contactForm.Email,
                        message = contactForm.Message,
                        reply_to = contactForm.Email,
                        g_recaptcha_response = contactForm.RecaptchaToken
                    }
                };

                var jsonContent = JsonSerializer.Serialize(emailData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://api.emailjs.com/api/v1.0/email/send", content);

                if (response.IsSuccessStatusCode)
                {
                    return Result.Success();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Result.Failure($"EmailJS API error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to send email via EmailJS: {ex.Message}");
            }
        }

        public Task<Result> SendEmailAsync(string to, string subject, string body)
        {
            // For direct email sending, we'd need to create a different EmailJS template
            // This method is kept for interface compatibility
            return Task.FromResult(Result.Failure("Direct email sending not implemented with EmailJS. Use SendContactEmail instead."));
        }
    }
}