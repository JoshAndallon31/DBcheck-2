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
                using (var sha256 = SHA256.Create())
                {
                    byte[] passwordHash = Encoding.UTF8.GetBytes(input.Password);
                    byte[] hashValue = sha256.ComputeHash(passwordHash);
                    string hashString = BitConverter.ToString(hashValue).Replace("-", "");
                    User.Password = hashString;
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
            var users = await _infosRepository.GetById(input.UserId);
            users.UserName = input.UserName;
            users.Password = input.Password;

            if (users == null)
            {
                return NotFound($"User with{input}");
            }
            if ( await _infosRepository.SaveAllChangesAsync())
            {
                return Ok("Update Successfully");
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
            var user = await _infosRepository.GetByUserName(username);

            if (user == null)
            {
                return Unauthorized();
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] passwordHash = Encoding.UTF8.GetBytes(password);
                byte[] hashValue = sha256.ComputeHash(passwordHash);
                string hashString = BitConverter.ToString(hashValue).Replace("-", "");

                if (user.Password != hashString)
                {
                    return Unauthorized();
                }
            }

            return Ok(user);
        }
        

    }
}