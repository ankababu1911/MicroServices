using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService=authService;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.LoginAsync(loginRequestDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto result = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));
                await SignInUser(result);
                _tokenProvider.SetToken(result.Token);
                return RedirectToAction("Index", "Home");
            }
            else {
                //ModelState.AddModelError("CustomError", responseDto.Message);
                TempData["error"] = responseDto.Message;
            }
            return View(loginRequestDto);
        }
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem> { new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin }, new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer } };
            ViewBag.RoleList= roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            ResponseDto result=await _authService.RegisterAsync(registrationRequestDto);
            ResponseDto assignRole;
            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(registrationRequestDto.Role))
                {
                    registrationRequestDto.Role = SD.RoleCustomer;
                }
                else
                {
                    registrationRequestDto.Role = SD.RoleAdmin;
                }
                assignRole = await _authService.AssignRoleAsync(registrationRequestDto);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else {
                TempData["error"] = result.Message;
            }
            var roleList = new List<SelectListItem> { new SelectListItem { Text = SD.RoleAdmin, Value = SD.RoleAdmin }, new SelectListItem { Text = SD.RoleCustomer, Value = SD.RoleCustomer } };
            ViewBag.RoleList = roleList;
            return View(registrationRequestDto);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index","Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token); // Use ReadJwtToken instead of ReadToken

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            if (emailClaim != null)
            {
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, emailClaim.Value));
                identity.AddClaim(new Claim(ClaimTypes.Name, emailClaim.Value));
            }

            var subClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (subClaim != null)
            {
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, subClaim.Value));
            }

            var nameClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            if (nameClaim != null)
            {
                identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, nameClaim.Value));
            }
            var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == "role");
            if (roleClaim != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
            }

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    }
}
