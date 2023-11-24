using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApiUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        ILogger<AuthController> logger,
        IMapper mapper,
        UserManager<ApiUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        _logger.LogInformation($"Registration attempt for {userDto.Email}");
        try
        {
            // test password: P@ssw0rd
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded == false)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return Accepted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginUserDto userDto)
    {
        _logger.LogInformation($"Login attempt for {userDto.Email}");
        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, userDto.Password);
            if (user == null || !isPasswordValid)
            {
                return Unauthorized();
            }

            string tokenString = await GenerateToken(user);

            var response = new AuthResponse
            {
                Email = userDto.Email,
                Token = tokenString,
                UserId = user.Id
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
            return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
        }
    }

    private async Task<string> GenerateToken(ApiUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
        var userClaims = await _userManager.GetClaimsAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(CustomClaimTypes.Uid, user.Id),
        }
        .Union(roleClaims)
        .Union(userClaims);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["JwtSettings:Duration"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
