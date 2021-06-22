using System.ComponentModel.DataAnnotations;

//That classes looks like simple model on mongodb

namespace API.DTOs
{
    public class RegisterDto
    {
        //We can add different validators, email, phone, etc ...
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}