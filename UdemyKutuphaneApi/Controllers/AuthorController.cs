using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UdemyKutuphaneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        //DI ile Servisi İnject Ettik
        private readonly IAuthorService _autherService;

        public AuthorController(IAuthorService autherService)
        {
            _autherService = autherService;
        }

        [AllowAnonymous]
        [HttpGet("ListAll")]
        public IActionResult GetAll() 
        { 
        
            var authors = _autherService.ListAll();

            if (!authors.IsSuccess)
            {
                return NotFound(authors.Message);
            }

            return Ok(authors);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _autherService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AuthorCreateDto author)
        {
            if (author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = _autherService.Create(author);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _autherService.GetByName(name);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _autherService.GetById(id);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("Update")]
        public IActionResult Update(AuthorUpdateDto authorUpdateDto) 
        { 
          
            if(authorUpdateDto == null) 
            {
                return BadRequest("Boş alan gönderilemez");
            }

            var result = _autherService.Update(authorUpdateDto).Result;

            if (!result.IsSuccess) 
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

    }
}
