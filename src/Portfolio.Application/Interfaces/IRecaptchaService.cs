using Portfolio.Domain.Common;

namespace Portfolio.Application.Interfaces
{
    public interface IRecaptchaService
    {
        Task<Result> ValidateRecaptcha(string token);
    }
}