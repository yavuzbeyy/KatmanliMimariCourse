using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UdemyKutuphaneApi.Controllers
{
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

        [HttpGet("ListAll")]
        public IActionResult GetAll() 
        { 
        
            var authors = _autherService.ListAll();

            if (!authors.IsSuccess)
            {
                return NotFound("Yazar bulunamadı.");
            }

            return Ok(authors);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _autherService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest("Silme İşlemi Başarısız Oldu");
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
                return BadRequest("Yazar oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _autherService.GetByName(name);

            if (!result.IsSuccess)
            {
                return BadRequest("Yazar bulunamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _autherService.GetById(id);

            if (!result.IsSuccess)
            {
                return NotFound("Yazar bulunamadı.");
            }

            return Ok(result);
        }

    }
}
