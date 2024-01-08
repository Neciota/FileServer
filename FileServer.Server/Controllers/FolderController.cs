using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IUserRequestService _userRequestService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IFolderService folderService, ILogger<FolderController> logger, IUserRequestService userRequestService)
        {
            _folderService = folderService;
            _logger = logger;
            _userRequestService = userRequestService;
        }

        [HttpGet(Name = "api/v1/folder")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TopFolderResponse>>> GetFoldersAsync()
        {
            ServiceResponse<IEnumerable<TopFolderResponse>> result = await _folderService.GetTopFoldersAsync(_userRequestService.User!.Guid);

            if (!result.Success)
                return BadRequest();

            return Ok(result.Data);
        }

        [HttpPost(Name = "api/v1/folder")]
        [Authorize]
        public async Task<ActionResult<FolderResponse>> CreateFolderAsync(CreateFolderRequest request)
        {
            ServiceResponse<FolderResponse> result = await _folderService.CreateFolderAsync(request, _userRequestService.User!.Guid);

            if (!result.Success)
                return BadRequest();

            return Ok(result.Data);
        }
    }
}
