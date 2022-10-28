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
    public class TransferController : ControllerBase
    {
        private readonly ITransferBusiness _transferBusiness;

        public TransferController(ITransferBusiness transferBusiness)
        {
            _transferBusiness = transferBusiness;
        }

        // POST api/<TransferController>
        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(TransferRequest transferRequest)
        {
            var idUser = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value!);
            if (idUser == 0)
                return Unauthorized("Authentication required");
            try
            {
                var result = await _transferBusiness.CreateTransferBusiness(transferRequest, idUser);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                switch(ex)
                {
                    case AuthenticationException:
                        return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
                    default:
                        return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
            }
        }
    }
}
