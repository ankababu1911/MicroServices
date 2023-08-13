namespace Mango.Services.AuthAPI.Models.DTO
{
    public class UserDto
    {
        public string ID { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
