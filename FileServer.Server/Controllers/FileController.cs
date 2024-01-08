using FileServer.Models.Requests;
using FileServer.Models.Responses;
using FileServer.Server.Models;
using FileServer.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUserRequestService _userRequestService;

        public FileController(IFileService fileService, IUserRequestService userRequestService)
        {
            _fileService = fileService;
            _userRequestService = userRequestService;
        }

        [HttpPost(Name = "api/v1/file")]
        [Authorize]
        public async Task<ActionResult<FileResponse>> UploadFileAsync([FromForm] UploadFileRequest request)
        {
            ServiceResponse<FileResponse> result = await _fileService.UploadFileAsync(request, request.File, _userRequestService.User!.Guid);

            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data);
        }

        [HttpGet(Name = "api/v1/file")]
        [Authorize]
        public async Task<IActionResult> DownloadFileAsync(DownloadFileRequest request)
        {
            ServiceResponse<(FileStream, string)> result = await _fileService.DownloadFileAsync(request, _userRequestService.User!.Guid);

            if (!result.Success)
                return BadRequest(result.FailureReason);

            return File(result.Data.Item1, "application/octet-stream", result.Data.Item2);
        }
    }
}
