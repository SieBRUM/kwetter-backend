using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Database;
using UserService.DTO;
using UserService.Helpers;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserServiceDatabaseContext _context;

        public AuthenticationController(UserServiceDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Username))
            {
                return BadRequest("LOGIN.MISSING_USERNAME");
            }

            if (string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return BadRequest("LOGIN.MISSING_PASSWORD");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginModel.Username && x.Password == loginModel.Password);
            if (user == null)
            {
                return BadRequest();
            }

            if (user.VerifyEmailToken != null)
            {
                // Maybe resent mail idk
                return Unauthorized("LOGIN.UNVERIFIED_ACCOUNT");
            }

            var token = new TokenBuilder().BuildToken(user.Id);
            var serializedToken = JsonSerializer.Serialize(token);
            return Ok(serializedToken);
        }

        [HttpPost("verifyemail")]
        public async Task<IActionResult> VerifyToken(VerifyEmailTokenModel verifyEmailTokenModel)
        {
            if (verifyEmailTokenModel == null)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == verifyEmailTokenModel.Username && x.Password == verifyEmailTokenModel.Password);
            
            if (user == null)
            {
                return BadRequest("VERIFY.INCORRECT_INFO");
            }
            
            if(user.VerifyEmailToken == null)
            {
                return BadRequest("VERIFY.ALREADY_VERIFIED");
            }

            if (user.VerifyEmailToken != verifyEmailTokenModel.VerifyEmailToken)
            {
                return BadRequest("VERIFY.INCORRECT_TOKEN");
            }

            user.VerifyEmailToken = null;
            await _context.SaveChangesAsync();
            return await Login(verifyEmailTokenModel);
        }

    }
}
