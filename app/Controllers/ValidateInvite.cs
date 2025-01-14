using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidateInviteController : ControllerBase
    {
        [HttpPost("validate-invite")]
        public IActionResult ValidateInvite([FromBody] InviteValidationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InviteCode))
            {
                return BadRequest("Incorrect, check out for spaces or null");
            }

            try
            {
                var validator = new InviteValidator();
                var isMatch = validator.ExactMatchCode(request.InviteCode);

                if (isMatch)
                {
                    return Ok(new { message = "Invite code matches." });
                }
                else
                {
                    return NotFound(new { message = "Invite code does not match." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ValidateInvite: {ex.Message}");
                return StatusCode(500, "An error occurred while validating the invite code.");
            }
        }
    }
}
