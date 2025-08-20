using Portfolio.Application.DTOs;
using Portfolio.Domain.Common;

namespace Portfolio.Application.Interfaces
{
    public interface IContactService
    {
        Task<Result> SubmitContactForm(ContactFormDto contactFormDto);
    }
}