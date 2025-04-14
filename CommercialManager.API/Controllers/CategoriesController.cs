using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialManager.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServices _services;
        public CategoriesController(ICategoriesServices services)
        {
            _services = services;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseDto<CategoryDto>>> PostCategory([FromBody] CategoryCreateDto dto)
        {
            var response = await _services.CreateCategoryAsync(dto);

            return StatusCode(response.StatusCode, response);
        }
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<CategoryDto>>>>> GetCategoryList(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _services.GetListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CategoryActionResponseDto>>> GetCategory(Guid id)
        {
            var response = await _services.GetOneByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<CategoryActionResponseDto>>> PutCategory(Guid id, [FromBody] CategoryEditDto dto)
        {
            var reponse = await _services.EditByIdAsync(id, dto);

            return StatusCode(reponse.StatusCode, reponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<CategoryActionResponseDto>>> DeleteCategory(Guid id)
        {
            var response = await _services.DeleteByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
