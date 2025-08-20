using Moq;
using NUnit.Framework;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using System.Threading.Tasks;

namespace Portfolio.Tests.Services
{
    [TestFixture]
    public class EmailServiceTests
    {
        private IEmailService _emailService;
        private Mock<IEmailService> _emailServiceMock;

        [SetUp]
        public void SetUp()
        {
            _emailServiceMock = new Mock<IEmailService>();
            _emailService = new EmailService(/* dependencies */);
        }

        [Test]
        public async Task SendEmail_ShouldReturnTrue_WhenEmailIsSentSuccessfully()
        {
            // Arrange
            var emailDto = new EmailDto { /* initialize with test data */ };
            _emailServiceMock.Setup(service => service.SendEmailAsync(emailDto)).ReturnsAsync(true);

            // Act
            var result = await _emailService.SendEmailAsync(emailDto);

            // Assert
            Assert.IsTrue(result);
            _emailServiceMock.Verify(service => service.SendEmailAsync(emailDto), Times.Once);
        }

        [Test]
        public async Task SendEmail_ShouldReturnFalse_WhenEmailSendingFails()
        {
            // Arrange
            var emailDto = new EmailDto { /* initialize with test data */ };
            _emailServiceMock.Setup(service => service.SendEmailAsync(emailDto)).ReturnsAsync(false);

            // Act
            var result = await _emailService.SendEmailAsync(emailDto);

            // Assert
            Assert.IsFalse(result);
            _emailServiceMock.Verify(service => service.SendEmailAsync(emailDto), Times.Once);
        }
    }
}