using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System.Security.Authentication;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentBusiness _documentBusiness;

        public DocumentController(IDocumentBusiness documentBusiness)
        {
            _documentBusiness = documentBusiness;
        }

        [HttpPost("{accountId:int}")]
        [RequestSizeLimit(2 * 1024 * 1024)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDownloadFile(IFormFile file, int accountId)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }
            try
            {
                var result = await _documentBusiness.DownloadFilel(file, userId, accountId);
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

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get( int id)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }
            try
            {
                var result = await _documentBusiness.Get(id, userId);

                MemoryStream ms = new MemoryStream(result);
                return new FileStreamResult(ms, "application/pdf");

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
        [HttpGet]

        [ProducesResponseType(typeof(IEnumerable<DocumentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }
            try
            {
                var result = await _documentBusiness.Get(userId);
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
