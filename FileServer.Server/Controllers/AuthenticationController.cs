using FileServer.Models.Requests;
using FileServer.Server.Models;
using FileServer.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthenticationController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(Name = "api/v1/login")]
        public async Task<ActionResult<string>> LoginAsync(LoginRequest request)
        {
            ServiceResponse<string> result = await _accountService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data);
        }
    }
}
