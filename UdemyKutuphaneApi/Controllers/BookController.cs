using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UdemyKutuphaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var books = _bookService.ListAll();

            if (!books.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(books);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _bookService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest("Silme İşlemi Başarısız Oldu");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] BookCreateDto book)
        {
            if (book == null)
            {
                return BadRequest("Kitap bilgileri boş olamaz.");
            }

            var result = _bookService.Create(book);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kitap oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _bookService.GetById(id);

            if (!result.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(result);
        }



    }
}
