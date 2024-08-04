using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.DTO;
using WebApi.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;


        public AccountController(UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager , IConfiguration _config ) 
        {
            userManager = _userManager;
            signInManager = _signInManager;
            config = _config; 
        }
        // create account new user "registration" 
        [HttpPost("register")]
        public async Task< IActionResult> CreateNewUser(RegisterUserDTO NewUser)
        {
            if(ModelState.IsValid == true) 
            {
                ApplicationUser User = new ApplicationUser();
                User.Email = NewUser.Email;
                User.UserName = NewUser.UserName;
                IdentityResult result = await userManager.CreateAsync(User, NewUser.Password);
                if(result.Succeeded) 
                {
                    return Ok("Succeeded saved");
                }
                else
                {
                    return BadRequest(result.Errors.FirstOrDefault());
                     
                }

            }
            return BadRequest(ModelState);

        }

        // check account valid "login "
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser existUser = await userManager.FindByNameAsync(userDto.UserName);
                if (existUser != null)
                {
                    SignInResult result = await signInManager.CheckPasswordSignInAsync(existUser, userDto.Password, false);
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existUser.UserName),
                    new Claim(ClaimTypes.NameIdentifier, existUser.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var roles = await userManager.GetRolesAsync(existUser);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],
                            audience: config["JWT:ValidAudience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: signingCredentials
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                    return Unauthorized("Invalid password.");
                }
                return Unauthorized("User does not exist.");
            }
            return Unauthorized("Invalid password.");
        }

    }
}
