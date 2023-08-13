using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;

        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user=_appDbContext.ApplicationUsers.First(x=>x.Email.ToLower()==email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                { 
                  _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _appDbContext.ApplicationUsers.FirstOrDefault(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = false; UserDto userDto = new(); LoginResponseDto loginResponseDto = new();
            if (user != null)
            {
                isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (isValid && !string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.PhoneNumber))
                {
                    userDto.Email = user.Email; userDto.ID = user.Id; userDto.Name = user.Name; userDto.PhoneNumber = user.PhoneNumber;
                    var token = _jwtTokenGenerator.GenerateToken(user);
                    loginResponseDto.User = userDto; loginResponseDto.Token = token;
                }
            }
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
                Name = registrationRequestDto.Name
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _appDbContext.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);
                    if (userToReturn != null)
                    {
                        UserDto userDto = new()
                        {
                            Email = userToReturn.Email,
                            ID = userToReturn.Id,
                            Name = userToReturn.Name,
                            PhoneNumber = userToReturn.PhoneNumber
                        };
                        return "";
                    }
                }

                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "Error Encountered";
        }
    }
}
