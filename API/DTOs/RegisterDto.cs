using System;
using System.ComponentModel.DataAnnotations;

//That classes looks like simple model on mongodb

namespace API.DTOs
{
    public class RegisterDto
    {
        //We can add different validators, email, phone, etc ...
        [Required]public string Username { get; set; }
        [Required] public string KnownAs { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public DateTime DateOfBirth { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Country { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}