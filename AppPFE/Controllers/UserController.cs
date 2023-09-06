using AppPFE.Context;
using AppPFE.Helpers;
using AppPFE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace AppPFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly AppDbContext authContext;
        public UserController(AppDbContext appDbContext)
        {
            authContext = appDbContext;

        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await authContext.Users
                .FirstOrDefaultAsync(x => x.NomAgence == userObj.NomAgence );
            if (user == null)
                return NotFound(new { Message = "user Not Found" });
            //verify hash password
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new {Message="Password is incorrect"});
            }
            return Ok(new
            {
                Message = "Login Success!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            //check NomAgence
            if (await checkUserNameExist(userObj.NomAgence))
                return BadRequest(new { Message = "Nom Agence existe!" });
            //check Email
            if (await checkEmailExist(userObj.Email))
                return BadRequest(new { Message = "Email existe!" });
            //check password strength
            var pwd = CheckPasswordStrength(userObj.Password);
            if(!string.IsNullOrEmpty(pwd))
                return BadRequest(new {Message = pwd.ToString() });
            //hash password
            userObj.Password = PasswordHasher.Hashpassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            //send to database
            await authContext.Users.AddAsync(userObj);
            //save on the database
            await authContext.SaveChangesAsync();
            return Ok(new
            {
                Message = "Register Success!"
            });

        }

        private Task<bool> checkUserNameExist(string NomAgence)
        {
            return authContext.Users.AnyAsync(x => x.NomAgence == NomAgence);

        }

        private Task<bool> checkEmailExist(string email)
        {
            return authContext.Users.AnyAsync(x => x.Email == email);

        }
        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
                sb.Append(" mot de passe doit contenir au minimum 8 charactére " + Environment.NewLine);
            if (!Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password,"[A-Z]") && Regex.IsMatch(password,"[0-9]"))
                sb.Append(" mot de passe doit etre alphanumérique " + Environment.NewLine);

            return sb.ToString();
        }


        [HttpGet]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await authContext.Users.ToListAsync());
        }
        

        }
    }


