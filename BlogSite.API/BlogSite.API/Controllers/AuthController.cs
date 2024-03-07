using BlogSite.API.Models.DTO;
using BlogSite.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager , ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        // POST :../api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginRequestDTO loginRequest)
        {
            // Check email
            var identityUser = await userManager.FindByEmailAsync(loginRequest.Email);

            if (identityUser != null)
            {
                // Check password
                var passwordChecked = await userManager.CheckPasswordAsync(identityUser, loginRequest.Password);

                if (passwordChecked)
                {
                    // Get User roles
                    var roles = await userManager.GetRolesAsync(identityUser);

                    // Create token and send back responce
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var res = new LoginResponseDTO
                    {
                        Email = loginRequest.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(res);
                }

            }

            ModelState.AddModelError("", "Email or Password Incorrect");
            return ValidationProblem(ModelState);

        }


        // POST : ../api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequestDTO registerRequest)
        {
            // create identity user
            var user = new IdentityUser
            {
                Email = registerRequest.Email?.Trim(),
                UserName = registerRequest.Email?.Trim(),
            };

            var identityResult = await userManager.CreateAsync(user,registerRequest.Password);

            if(identityResult.Succeeded)
            {
                // Add role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if(identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if(identityResult.Errors.Any())
                    {
                        foreach(var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
