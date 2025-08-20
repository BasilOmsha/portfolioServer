using Microsoft.AspNetCore.Mvc;
using Moq;
using Portfolio.Api.Controllers;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace Portfolio.Tests.Controllers
{
    public class ContactControllerTests
    {
        private readonly Mock<IContactService> _contactServiceMock;
        private readonly Mock<IRecaptchaService> _recaptchaServiceMock;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _contactServiceMock = new Mock<IContactService>();
            _recaptchaServiceMock = new Mock<IRecaptchaService>();
            _controller = new ContactController(_contactServiceMock.Object, _recaptchaServiceMock.Object);
        }

        [Fact]
        public async Task SubmitContactForm_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var contactFormDto = new ContactFormDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Message = "Hello!"
            };

            _recaptchaServiceMock.Setup(r => r.ValidateRecaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.SubmitContactForm(contactFormDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SubmitContactForm_InvalidRecaptcha_ReturnsBadRequest()
        {
            // Arrange
            var contactFormDto = new ContactFormDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Message = "Hello!"
            };

            _recaptchaServiceMock.Setup(r => r.ValidateRecaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.SubmitContactForm(contactFormDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task SubmitContactForm_ServiceThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var contactFormDto = new ContactFormDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Message = "Hello!"
            };

            _recaptchaServiceMock.Setup(r => r.ValidateRecaptchaAsync(It.IsAny<string>()))
                .ReturnsAsync(true);
            _contactServiceMock.Setup(s => s.SendContactFormAsync(contactFormDto))
                .ThrowsAsync(new System.Exception("Service error"));

            // Act
            var result = await _controller.SubmitContactForm(contactFormDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}