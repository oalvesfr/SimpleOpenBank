using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOpenBank.Application.Contracts.Business;
using System.Security.Authentication;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentBusiness _documentBusiness;

        public DocumentController(IDocumentBusiness documentBusiness)
        {
            _documentBusiness = documentBusiness;
        }

        [HttpPost("{accountId:int}")]
        [RequestSizeLimit(2 * 1024 * 1024)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostUploadFile(IFormFile file, int accountId)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value!);
            if (userId == 0) return Unauthorized("Authentication required");
            try
            {
                var result = await _documentBusiness.UploadFile(file, userId, accountId);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return ex switch
                {
                    ArgumentException => StatusCode(StatusCodes.Status400BadRequest, ex.Message),
                    AuthenticationException => StatusCode(StatusCodes.Status401Unauthorized, ex.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, ex.Message)
                };
            }
        }
    }
}
