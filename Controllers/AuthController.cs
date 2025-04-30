using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using piece_of_iceland_api.DTOs;
using piece_of_iceland_api.Services;
using piece_of_iceland_api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace piece_of_iceland_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IConfiguration _config;

    public AuthController(UserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var user = (await _userService.GetAsync()).FirstOrDefault(u => u.Email == login.Email);
        if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = GenerateJwtToken(user.Id, user.Email);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto register)
    {
        var existingUser = (await _userService.GetAsync())
            .FirstOrDefault(u => u.Email == register.Email);

        if (existingUser is not null)
        {
            return Conflict("User with this email already exists.");
        }

        var user = new User
        {
            Email = register.Email,
            Username = register.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password)
        };

        await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(Login), new { email = user.Email }, new
        {
            user.Id,
            user.Email,
            user.Username
        });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            userId,
            email
        });
    }

    [Authorize]
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        if (userId is null || email is null)
        {
            return Unauthorized();
        }

        var newToken = GenerateJwtToken(userId, email);
        return Ok(new { token = newToken });
    }

    private string GenerateJwtToken(string userId, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        Console.WriteLine($"🔑 Creating JWT for {email} ({userId})");

        var claims = new[]
        {
            new Claim("sub", userId),
            new Claim("email", email),
            new Claim("jti", Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
