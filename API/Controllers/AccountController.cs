using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Data;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext context;
        private readonly ITokenService tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountController(DataContext context, ITokenService tokenService, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            this.context = context;
            this.tokenService = tokenService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

            var newUser = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            newUser.UserName = registerDto.Username.ToLower();
            newUser.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            newUser.PasswordSalt = hmac.Key;

            // var user = createUser(registerDto.Username, registerDto.Password);

            saveUserInDB(newUser);

            return new UserDto
            {
                Username = newUser.UserName,
                Token = tokenService.CreateToken(newUser),
                KnownAs = newUser.KnownAs
            }; 
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userRepository.GetUserByUserNameAsync(loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!checkPasswordIsCorrect(computedHash, user.PasswordHash)) return Unauthorized("Invalid password");

            return new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs
            };
        }

        public UserDto createUserDto(AppUser user)
        {
            return new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            };
        }

        private bool checkPasswordIsCorrect(byte[] computedHash, byte[] currentPasswordHash)
        {

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != currentPasswordHash[i]) return false;
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