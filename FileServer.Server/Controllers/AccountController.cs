using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(Name = "api/v1/account")]
        public async Task<ActionResult<UserResponse>> RegisterAccountAsync(RegisterRequest request)
        {
            ServiceResponse<UserResponse> result = await _accountService.RegisterUserAsync(request);

            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data);
        }
    }
}
