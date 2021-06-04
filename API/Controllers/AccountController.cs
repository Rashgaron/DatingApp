using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext context;

        public AccountController(DataContext context)
        {
            this.context = context;
        }


        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");
            
            var user = createUser(registerDto.Username, registerDto.Password); 

            saveUserInDB(user);

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {

            var user = await context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if(user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt); 

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if(!checkPasswordIsCorrect(computedHash, user.PasswordHash)) return Unauthorized("Invalid password");

            return user;

        }

        private bool checkPasswordIsCorrect(byte[] computedHash, byte[] currentPasswordHash)
        {

            for(int i = 0; i < computedHash.Length ; i++)
            {
                if(computedHash[i] != currentPasswordHash[i]) return false; 
            }

            return true;

        }


        private async void saveUserInDB(AppUser user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }        

        private AppUser createUser(string username, string password)
        {

            using var hmac = new HMACSHA512();

            var newUser = new AppUser
            {
                UserName = username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key 

            };
            return newUser;    
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }

}