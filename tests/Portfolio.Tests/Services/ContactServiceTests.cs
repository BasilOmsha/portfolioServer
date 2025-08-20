using Moq;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using Portfolio.Domain.Common;
using System.Threading.Tasks;
using Xunit;

namespace Portfolio.Tests.Services
{
    public class ContactServiceTests
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IRecaptchaService> _recaptchaServiceMock;
        private readonly ContactService _contactService;

        public ContactServiceTests()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _recaptchaServiceMock = new Mock<IRecaptchaService>();
            _contactService = new ContactService(_emailServiceMock.Object, _recaptchaServiceMock.Object);
        }

        [Fact]
        public async Task SubmitContactForm_ValidData_SendsEmail()
        {
            // Arrange
            var contactFormDto = new ContactFormDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Message = "Hello!"
            };

            _recaptchaServiceMock.Setup(x => x.ValidateRecaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(new RecaptchaResponseDto { Success = true });

            // Act
            var result = await _contactService.SubmitContactForm(contactFormDto, "recaptchaToken");

            // Assert
            Assert.True(result.IsSuccess);
            _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task SubmitContactForm_InvalidRecaptcha_ReturnsError()
        {
            // Arrange
            var contactFormDto = new ContactFormDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Message = "Hello!"
            };

            _recaptchaServiceMock.Setup(x => x.ValidateRecaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(new RecaptchaResponseDto { Success = false });

            // Act
            var result = await _contactService.SubmitContactForm(contactFormDto, "recaptchaToken");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid reCAPTCHA. Please try again.", result.ErrorMessage);
            _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}