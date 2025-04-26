using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UdemyKutuphaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Create")]
        public IActionResult CreateUser([FromBody] UserCreateDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.CreateUser(user);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult LoginUser([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.LoginUser(user);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }
    }
}
