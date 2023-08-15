using System.ComponentModel.DataAnnotations;

namespace Mango.web.Models
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
