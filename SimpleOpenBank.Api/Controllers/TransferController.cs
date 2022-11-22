using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using System.Security.Authentication;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        private readonly ITransferBusiness _transferBusiness;

        public TransferController(ITransferBusiness transferBusiness)
        {
            _transferBusiness = transferBusiness;
        }

        // POST api/<TransferController>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(TransferRequest transferRequest)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }
            try
            {
                var result = await _transferBusiness.CreateTransferBusiness(transferRequest, userId);
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
