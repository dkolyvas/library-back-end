using library_app.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Microsoft.Extensions.Options;
using library_app.Services;
using library_app.DTO;
using library_app.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;

namespace library_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IAppServices services;
        private IConfiguration configuration;
        private ILogger<UsersController> _logger;
        public string? Errors;

        public UsersController(IAppServices services, IConfiguration configuration, ILogger<UsersController> logger)
        {
            this.services = services;
            this.configuration = configuration;
            this._logger = logger;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO credentials)
        {
            if (credentials.Username is null || credentials.Password is null) 
            {
                return BadRequest("You must provide a username and password");
            }
            User? user = null;
            if (credentials.Username == "admin")
            {
                if (credentials.Password == configuration.GetValue<string>("AdminPassword"))
                {
                    user = new User() { Id = 0, Username = "admin", Password = credentials.Password, Name ="", Surname = "Administrator" };
                }
                else return BadRequest("Invalid admin password");
            }
            else
            {
                try
                {
                    user = await services.UserService.Login(credentials);
                  
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex.Message);
                    if (ex is EntityNotFoundException || ex is IncorrectPasswordException)
                    {
                        return Unauthorized(ex.Message);
                    }
                    else return Problem(ex.Message);
                }
            }
            string key = CreateUserToken(user.Id, user.Username);
            return Ok(new {key = key});
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<UserShowDTO>> Register(UserRegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                foreach(var value in ModelState.Values)
                {
                    foreach(var err in value.Errors)
                    {
                        Errors += err.ErrorMessage + " | ";
                    }
                }
                return BadRequest(Errors);
            }
            try
            {
                var result = await services.UserService.RegisterUser(registerDTO);
                return CreatedAtAction("GetUser",new { username = result.Username }, result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                if(ex is UserExistsException)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return Problem(ex.Message);
                }
            }
        }
        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<UserShowDTO>> GetUser(string username)
        {
            try
            {
                var result = await services.UserService.GetUserByUsername(username);
                return Ok(result);
            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<UserShowDTO>>> GetAll()
        {
            try
            {
                var results = await services.UserService.GetAllUsers();
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpPut("{username}")]
        [Authorize]
        public async Task<ActionResult<UserShowDTO>> UpdateUser(string username, UserUpdateDTO updateDTO)
        {
            if(username != updateDTO.Username)
            {
                return Unauthorized("Access is not allowed");
            }
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var err in value.Errors)
                    {
                        Errors += err.ErrorMessage + " | ";
                    }
                }
                return BadRequest(Errors);
            }

            try
            {
                var result = await services.UserService.UpdateUser(updateDTO);
                return Ok(result);
            }
            catch(IncorrectPasswordException e)
            {
                _logger.LogError(e.Message);
                return Unauthorized(e.Message);
            }
            catch(UnableToConfirmPasswordException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return Problem(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await services.UserService.DeleteUser(id);
                if (!result) Problem("Unable to save data");
                return Ok();

            }
            catch(EntityNotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }
        



        public string CreateUserToken(int userId, string username)
        {
            
            string appSecurityKey = configuration.GetValue<string>( "AppSecurityKey")!;
            
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claimsInfo = new List<Claim>();
            var tokenIssuer = "https://localhost:5001";

            claimsInfo.Add(new Claim(ClaimTypes.Name, username));
            if (username == "admin")
            {
                claimsInfo.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            else
            {
                claimsInfo.Add(new Claim(ClaimTypes.Role, "user"));
            }
            

            claimsInfo.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));



            var jwtSecurityToken = new JwtSecurityToken(tokenIssuer, null, claimsInfo, DateTime.UtcNow, DateTime.UtcNow.AddHours(3), signingCredentials);

            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return userToken; //"Bearer " + userToken;

        }
    }
}
