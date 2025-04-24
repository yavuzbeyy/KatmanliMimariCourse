using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UdemyKutuphaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var categories = _categoryService.ListAll();

            if (!categories.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(categories);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest("Silme İşlemi Başarısız Oldu");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryCreateDto category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgileri boş olamaz.");
            }

            var result = _categoryService.Create(category);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kategori oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var result = _categoryService.GetById(id);

            if (!result.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(result);
        }


    }
}
