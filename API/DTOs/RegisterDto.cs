using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        //We can add different validators, email, phone, etc ...
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}