using FlightDocumentManagementSystem.Helpers;
using FlightDocumentManagementSystem.Models;
using FlightDocumentManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocumentManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DisplaysController : ControllerBase
    {
        private readonly IDisplayRepository _displayRepository;

        public DisplaysController(IDisplayRepository displayRepository)
        {
            _displayRepository = displayRepository;
        }

        // GET: api/Display
        [HttpGet]
        public async Task<ActionResult<Display>> GetDisplays()
        {
            var result = await _displayRepository.GetAllDisplaysAsync();
            return Ok(new Notification
            {
                Success = true,
                Message = "Get Successfully",
                Data = result
            });
        }

        // PUT: api/Display/UpdateLogo
        [HttpPut("UpdateLogo")]
        public async Task<IActionResult> PutLogo(IFormFile logo)
        {
            var display = await _displayRepository.GetAllDisplaysAsync();
            if (display.Count == 0)
            {
                var resultI = await _displayRepository.InsertDisplayAsync(logo, false);
                return Ok(new Notification
                {
                    Success = true,
                    Message = "Update Successfully",
                    Data = resultI
                });
            }

            var resultU = await _displayRepository.UpdateLogoAsync(logo, display[0]);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = resultU
            });
        }

        // PUT: api/Display/UpdateCaptcha
        [HttpPut("UpdateCaptcha")]
        public async Task<IActionResult> PutCaptcha(bool status)
        {
            var display = await _displayRepository.GetAllDisplaysAsync();
            if (display.Count == 0)
            {
                var resultI = await _displayRepository.InsertDisplayAsync(null!, status);
                return Ok(new Notification
                {
                    Success = true,
                    Message = "Update Successfully",
                    Data = resultI
                });
            }

            var resultU = await _displayRepository.UpdateCaptchaAsync(status, display[0]);
            return Ok(new Notification
            {
                Success = true,
                Message = "Update Successfully",
                Data = resultU
            });
        }
    }
}
