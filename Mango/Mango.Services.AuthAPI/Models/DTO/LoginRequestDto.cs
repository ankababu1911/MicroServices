namespace Mango.Services.AuthAPI.Models.DTO
{
    public class LoginRequestDto
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
