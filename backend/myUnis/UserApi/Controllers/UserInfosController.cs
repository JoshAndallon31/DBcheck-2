using System.Runtime.Intrinsics.Arm;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserApi.Data;
using UserApi.Dto;
using UserApi.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace UserApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserInfosController : ControllerBase
    {
        private readonly ILogger<UserInfosController> _logger;
        private readonly IUserInfosRepository _infosRepository;

        public UserInfosController(ILogger<UserInfosController> logger, IUserInfosRepository infosRepository)
        {
            _logger = logger;
            _infosRepository = infosRepository;
        }
        
        [HttpGet()]
        public async Task<IActionResult> GetList()
        {
            var Users = await _infosRepository.GetAllAsync();
            if(Users == null || !Users.Any())
            {
                return Ok("Empty");
            }
            _logger.LogInformation("Getting all list");
            return Ok(Users);
        }



        [HttpPost()]
        public async Task<IActionResult> Post(UserInfoDto input)
        {
            try
            {


                var User = new UserInfos(input.UserId);
                User.UserName = input.UserName;
                byte []salt = new byte[16];
                RandomNumberGenerator.Fill(salt);
                
                
                using (var sha256 = SHA256.Create())
                {
                    byte[] saltedPassword = new byte[salt.Length + Encoding.UTF8.GetBytes(input.Password).Length];
                    salt.CopyTo(saltedPassword, 0);
                    Encoding.UTF8.GetBytes(input.Password).CopyTo(saltedPassword, salt.Length);
                    byte[] hashedPassword = sha256.ComputeHash(saltedPassword);
                    string saltString = Convert.ToBase64String(salt);
                    string hashedPasswordString = Convert.ToBase64String(hashedPassword);
                    User.Password = hashedPasswordString;
                    User.PasswordSalt = saltString;
                    _infosRepository.Add(User);
                    if (await _infosRepository.SaveAllChangesAsync())
                    {
                        return Ok("User Created Successfully");
                    }
                }

                return BadRequest("Error");

            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Error: A unique constraint would be violated it could be either the username or id has been used.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(UserInfoDto input)
        {
            var user = await _infosRepository.GetById(input.UserId);

            if (user == null)
            {
                return NotFound($"User with ID {input.UserId} not found.");
            }

            using (var sha256 = SHA256.Create())
            {
                // Generate a new random salt value
                byte[] salt = new byte[16];
                RandomNumberGenerator.Fill(salt);

                // Concatenate salt and password
                byte[] saltedPassword = new byte[salt.Length + Encoding.UTF8.GetBytes(input.Password).Length];
                salt.CopyTo(saltedPassword, 0);
                Encoding.UTF8.GetBytes(input.Password).CopyTo(saltedPassword, salt.Length);

                // Hash the salted password using SHA256
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                // Convert salt and hashed password to Base64 strings
                string saltString = Convert.ToBase64String(salt);
                string hashedPasswordString = Convert.ToBase64String(hashedPassword);

                // Store the salt and hashed password in the user object
                user.UserName = input.UserName;
                user.PasswordSalt = saltString;
                user.Password = hashedPasswordString;

                // Update the user object in the database
                if (await _infosRepository.SaveAllChangesAsync())
                {
                    return Ok("User updated successfully.");
                }
            }

            return BadRequest("Error updating user.");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var users = await _infosRepository.GetById(id);
            
            if (users != null) 
            {
                _infosRepository.Delete(users);
                if ( await _infosRepository.SaveAllChangesAsync())
                {
                    return Ok($"User with id {id} is deleted");
                }
            }


            return BadRequest($"User with id {id} is not found.");
        }
        [HttpGet]
        [Route("LoginUser/{username}/{password}")]
        public async Task<ActionResult<UserInfos>>LoginUser(string username, string password)
        {
            byte []salt = new byte[16];
            RandomNumberGenerator.Fill(salt);

            var user = await _infosRepository.GetByUserName(username);

                if (user == null)
                {
                    return Unauthorized();
                }

                using (var sha256 = SHA256.Create())
                {
                    // Retrieve the salt and hashed password from the database
                    byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt);
                    byte[] storedPassword = Convert.FromBase64String(user.Password);

                    // Concatenate salt and password
                    byte[] saltedPassword = new byte[storedSalt.Length + Encoding.UTF8.GetBytes(password).Length];
                    storedSalt.CopyTo(saltedPassword, 0);
                    Encoding.UTF8.GetBytes(password).CopyTo(saltedPassword, storedSalt.Length);

                    // Hash the salted password using SHA256
                    byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                    // Convert salt and hashed password to Base64 strings
                    string hashedPasswordString = Convert.ToBase64String(hashedPassword);
                    string storedPasswordString = Convert.ToBase64String(storedPassword);

                    // Check if the password matches the one stored in the database
                    if (hashedPasswordString == storedPasswordString)
                    {
                        return Ok(user);
                    }
                }

                return Unauthorized();
            }


    }
}