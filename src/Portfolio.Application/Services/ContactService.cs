using System.Threading.Tasks;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IEmailService _emailService;
        private readonly IRecaptchaService _recaptchaService;

        public ContactService(IEmailService emailService, IRecaptchaService recaptchaService)
        {
            _emailService = emailService;
            _recaptchaService = recaptchaService;
        }

        public async Task<Result> SubmitContactForm(ContactFormDto contactFormDto)
        {
            var recaptchaResult = await _recaptchaService.ValidateRecaptcha(contactFormDto.RecaptchaToken);
            if (!recaptchaResult.IsSuccess)
            {
                return Result.Failure("Invalid reCAPTCHA.");
            }

            var contactForm = new ContactForm
            {
                Name = contactFormDto.Name,
                Email = contactFormDto.Email,
                Message = contactFormDto.Message,
                RecaptchaToken = contactFormDto.RecaptchaToken
            };

            // Logic to save contactForm to the database can be added here

            var emailResult = await _emailService.SendContactEmail(contactForm);
            if (!emailResult.IsSuccess)
            {
                return Result.Failure("Failed to send email.");
            }

            return Result.Success();
        }
    }
}