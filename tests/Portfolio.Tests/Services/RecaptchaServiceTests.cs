using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Portfolio.Application.Interfaces;
using Portfolio.Application.Services;
using Portfolio.Application.DTOs;

namespace Portfolio.Tests.Services
{
    [TestFixture]
    public class RecaptchaServiceTests
    {
        private Mock<IRecaptchaService> _recaptchaServiceMock;
        private RecaptchaService _recaptchaService;

        [SetUp]
        public void SetUp()
        {
            _recaptchaServiceMock = new Mock<IRecaptchaService>();
            _recaptchaService = new RecaptchaService(_recaptchaServiceMock.Object);
        }

        [Test]
        public async Task ValidateRecaptcha_ShouldReturnTrue_WhenValidToken()
        {
            // Arrange
            var token = "valid_token";
            _recaptchaServiceMock.Setup(x => x.ValidateRecaptchaAsync(token)).ReturnsAsync(true);

            // Act
            var result = await _recaptchaService.ValidateRecaptchaAsync(token);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ValidateRecaptcha_ShouldReturnFalse_WhenInvalidToken()
        {
            // Arrange
            var token = "invalid_token";
            _recaptchaServiceMock.Setup(x => x.ValidateRecaptchaAsync(token)).ReturnsAsync(false);

            // Act
            var result = await _recaptchaService.ValidateRecaptchaAsync(token);

            // Assert
            Assert.IsFalse(result);
        }
    }
}