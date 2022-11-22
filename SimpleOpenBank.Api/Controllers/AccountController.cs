using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Models;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using SimpleOpenBank.Domain;
using System.Net;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBusiness _accountBusiness;
        public AccountController(IAccountBusiness accountBusiness)
        {
            _accountBusiness = accountBusiness;
        }

        // GET: <AccountController>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<List<AccountResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAccounts()
        {
            if(!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }

            try
            {
                var account = await _accountBusiness.Get(userId);
                return Ok(account);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        // GET <AccountController>/5
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(IEnumerable<AccountMovims>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAccountById(int id)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }

            try
            {
                var account = await _accountBusiness.Get(userId, id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    UnauthorizedAccessException => StatusCode(StatusCodes.Status401Unauthorized, ex.Message),
                    _ => StatusCode(StatusCodes.Status404NotFound, ex.Message)
                };

            }
        }

        // POST <AccountController>
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostCreatedAccount(AccountRequest accountRequest)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value, out int userId))
            { return Unauthorized("Authentication required"); }
            try
            {
                var accountResponse = await _accountBusiness.Create(accountRequest, userId);
                return Ok(accountResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
