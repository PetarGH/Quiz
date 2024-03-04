using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IManagers;
using Domain.Entities;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Infrastructure.IHelpers;
using Infrastructure.ErrorResponse;
using Infrastructure.Response;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController>? logger, IUserManager userManager, IJwtService? jwtService)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _userManager.GetUserByID(id);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "User not found",
                    Code = 404
                });
            }

            var userDto = new ResponseUserBody
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                UserType = user.UserType,
                Age = user.Age,
                IsFrozen = user.IsFrozen,
            };

            return Ok(userDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<IpUser>>> GetAllUsers()
        {
            List<IpUser> users = _userManager.GetAllUsers();
            if (users == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "User not found",
                    Code = 404
                });
            }

            List<ResponseUserBody> userDtos = users.Select(user => new ResponseUserBody
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                UserType = user.UserType,
                Age = user.Age,
                IsFrozen = user.IsFrozen,

            }).ToList();

            return Ok(userDtos);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the email is already taken
            if (await _userManager.IsEmailTaken(registerUser.Email))
            {
                return BadRequest("Email is already taken.");
            }
            // Check if the user is added to the database
            bool result = _userManager.RegisterUser(registerUser);
            if (result)
            {
                return Ok("Registered successfully.");
            }
            else return BadRequest(ModelState);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id,[FromBody] UpdateUserModel userModel)
        {

            var existingUser = _userManager.GetUserByID(id);
            if (existingUser == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "User not found",
                    Code = 404
                });
            }
            try
            {
                existingUser.Name = userModel.Name;
                existingUser.Address = userModel.Address;
                existingUser.Age = userModel.Age;

                _userManager.UpdateUser(existingUser);

                return Ok("User is updated successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse 
                {
                    Message = ex.Message,
                    Code = 4005
                });
            }
        }

        [HttpPut("Freeze/{id}")]
        public async Task<IActionResult> FreezeUserAccount(int id)
        {
            try
            {
                bool success = await _userManager.FreezeUser(id);

                if (success)
                {
                    return Ok("User account frozen successfully.");
                }
                else
                {
                    return NotFound(new ErrorResponse
                    {
                        Message = "User not found",
                        Code = 404
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while freezing the user account.");
            }
        }

        //This method is for the user to delete his own account if needed.
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                bool result = await _userManager.DeleteUser(id);
                if (result)
                {
                    return Ok("Your account is deleted!");
                }
                else
                {
                    return BadRequest("Something went wrong.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured while deleting the user account");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var loggeduser = await _userManager.LoginAsync(loginModel.Email, loginModel.Password);
            if (loggeduser == null)
            {
                return BadRequest("Invalid email or password");
            }

            var jwt = _jwtService.Generate(loggeduser.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
            });

            return Ok("success");
        }


        /// <summary>
        /// this method will return the logged user.
        /// </summary>
        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetLoggedUser()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verfiy(jwt);

                int userID = int.Parse(token.Issuer);

                var user = GetUser(userID);

                return Ok(user);
            }
            catch(Exception _)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            };

            Response.Cookies.Append("jwt", "", cookieOptions);

            return Ok("success");
        }

        [HttpGet]
        [Route("CheckJwtToken")]
        public IActionResult CheckJwtToken()
        {
            string jwtToken = Request.Cookies["jwt"];

            if (string.IsNullOrEmpty(jwtToken))
            {
                return Ok(new { HasToken = false });
            }
            else
            {
                return Ok(new { HasToken = true });
            }
        }
    }
}
