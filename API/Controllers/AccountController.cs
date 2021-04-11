using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.interfaces;

namespace API.Controllers
{
  public class AccountController : BaseAPIController
  {
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
      _tokenService = tokenService;
      _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {

      if (await UserExist(registerDto.Username)) return BadRequest("Username is taken.");

      using var hmac = new HMACSHA512();
      var user = new AppUser
      {
        UserName = registerDto.Username.ToLower(),
        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        PasswordSalt = hmac.Key
      };

      _context.Users.Add(user);

      await _context.SaveChangesAsync();

      return new UserDto
      {
        Username = user.UserName,
        Token = _tokenService.createToken(user)
      };
    }

    private async Task<bool> UserExist(string username)
    {
      return await _context.Users.AnyAsync(User => User.UserName == username.ToLower());
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _context.Users.SingleOrDefaultAsync(User => User.UserName == loginDto.Username);
      if (user == null)
      {
        return Unauthorized("Invalid username");
      }

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      bool isMatch = true;
      for (int i = 0; i < computedHash.Length; i++)
      {
        if (user.PasswordHash[i] != computedHash[i])
        {
          isMatch = false;
        }
      }

      return isMatch ? new UserDto 
      {
        Username = user.UserName,
        Token = _tokenService.createToken(user)
      } : Unauthorized("Invalid password.");
    }
  }
}