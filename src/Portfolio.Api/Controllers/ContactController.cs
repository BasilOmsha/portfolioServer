using Microsoft.AspNetCore.Mvc;
using Portfolio.Application.DTOs;
using Portfolio.Application.Interfaces;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Creates a new contact form submission
        /// POST /api/contact
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactFormDto contactFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { 
                    message = "Validation failed",
                    errors = ModelState.Where(x => x.Value?.Errors.Count > 0)
                           .ToDictionary(
                               kvp => kvp.Key,
                               kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                           )
                });
            }

            var result = await _contactService.SubmitContactForm(contactFormDto);
            
            if (result.IsSuccess)
            {
                return Created(string.Empty, new { 
                    message = "Message sent successfully!"
                });
            }

            return BadRequest(new { 
                message = "Failed to send message",
                error = result.ErrorMessage 
            });
        }
    }
}