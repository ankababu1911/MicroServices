using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Mango.Services.CouponAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthApiController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            var errorMessage= await _authService.Register(registrationRequestDto);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message=errorMessage;
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            return Ok(_responseDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var loginResponse = await _authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "username or password incorrect";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            _responseDto.Result = loginResponse;
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegistrationRequestDto registrationRequestDto)
        {
            var assignRolesuccessful = await _authService.AssignRole(registrationRequestDto.Email, registrationRequestDto.Role.ToUpper());
            if (!assignRolesuccessful)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Role is not assigned";
                return BadRequest(_responseDto);
            }
            _responseDto.IsSuccess = true;
            return Ok(_responseDto);
        }
    }
   
}
