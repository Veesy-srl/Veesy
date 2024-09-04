using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Veesy.Domain.Constants;
using Veesy.Domain.Models;
using Veesy.Presentation.Helper;
using Veesy.Presentation.Response.Auth;
using Veesy.Service.Dtos;
using Veesy.WebApp.CustomDataAttribute;

namespace Veesy.WebApp.Api.v1;


[ApiController]
[Route("api/v1/auth/{action}")]
public class AuthController : ControllerBase
{
    private readonly AuthHelper _authHelper;
    private readonly SignInManager<MyUser> _signInManager;
    private readonly UserManager<MyUser> _userManager;
    private readonly IConfiguration _config;

    public AuthController(AuthHelper authHelper, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager, IConfiguration config)
    {
        _authHelper = authHelper;
        _signInManager = signInManager;
        _userManager = userManager;
        _config = config;
    }

    [HttpGet]
    public async Task<IActionResult> Login(string username, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return StatusCode(401, new LoginResponse(-1, "Username or password are invalid"));
            }

            
            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    return StatusCode(401, new LoginResponse(-1, "Username or password are invalid"));
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user,password, false, lockoutOnFailure: false);
            
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return StatusCode(200, new LoginResponse(new LoginDto
                {
                    Token = token,
                    User = MapProfileDtos.MapUserDetailDto(user)
                }));
            }

            return StatusCode(401, new LoginResponse(-1, "Username or password are invalid"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new LoginResponse(-1,"Fatal error"));
        }
    }
    
    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(_config["Jwt:Expires"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}