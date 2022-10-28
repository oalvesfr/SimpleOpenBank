using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOpenBank.Application.Contracts.Business;
using SimpleOpenBank.Application.Contracts.Persistence;
using SimpleOpenBank.Application.Models.Requests;
using SimpleOpenBank.Application.Models.Responses;
using System.Runtime.CompilerServices;

namespace SimpleOpenBank.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness;
        }

        // POST api/<UserController>

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CreateUserResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateUserRequest userRequest)
        {

            try
            {
                var createUserResponse = await _userBusiness.CreatedUserBusiness(userRequest);
                return Created("User created", createUserResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }


        [HttpPost("Login")]
        [ProducesResponseType(typeof(IEnumerable<LoginUserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostLogin(LoginUserRequest loginUser)
        {
            if (loginUser.Username == null || loginUser.Password == null)
                return BadRequest("Username and Password required");
            try
            {
                var loginUserResponse = await _userBusiness.LoginBusiness(loginUser);
                return Ok(loginUserResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            

        }

        [HttpPost("RefreshToken")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> PostRefreshToken(string tokenRefresh)
        {
            try
            {
                var userResponse = await _userBusiness.RefreshTokenBusiness(tokenRefresh);
                return StatusCode(StatusCodes.Status201Created, userResponse);
            }
            catch (Exception ex)
            {
                switch(ex)
                {
                    case ArgumentNullException:
                        return StatusCode(StatusCodes.Status404NotFound, ex.Message);
                    default:
                        return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
                
            }

        }
    }
}
