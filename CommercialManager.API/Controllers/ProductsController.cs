using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Products;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialManager.API.Controllers
{

    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsServices _productsServices;

        public ProductsController( IProductsServices productsServices)
        {
            _productsServices = productsServices;
        }

        //Creation
        [HttpPost]
        public async Task<ActionResult<ResponseDto<ProductsActionResponseDto>>> Post([FromBody] ProductsCreateDto dto)
        {
            var response = await _productsServices.CreateAsync(dto);

            return StatusCode(response.StatusCode, new
            {
                response.Status,
                response.Message,
                response.Data
            });
        }

        //List
        [HttpGet]
        public async Task<ActionResult<ResponseDto<PaginationDto<List<ProductsDto>>>>> List(
        string searchTerm = "", int page = 1, int pageSize = 0)
        {
            var response = await _productsServices.GetListAsync(searchTerm, page, pageSize);

            return StatusCode(response.StatusCode, response);
        }

        //Get One 


        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<ProductsActionResponseDto>>> GetCategory(Guid id)
        {
            var response = await _productsServices.GetOneByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        //Edit

        [HttpPut("{id}")] 
        public async Task<ActionResult<ResponseDto<ProductsActionResponseDto>>> Edit([FromBody] ProductsEditDto dto, Guid id)
        {
            var response = await _productsServices.EditAsync(dto, id);

            return StatusCode(response.StatusCode, response);
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<ProductsActionResponseDto>>> Delete(Guid id)
        {
            var response = await _productsServices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
