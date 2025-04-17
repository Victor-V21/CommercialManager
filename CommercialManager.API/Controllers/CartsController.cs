using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialManager.API.Controllers
{
    [ApiController]
    [Route("/api/carts")]
    public class CartsController : ControllerBase
    {
        private readonly IShoppingCartsServices _services;
        public CartsController(IShoppingCartsServices services)
        {
            _services = services;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> AddItems(Guid id, [FromBody]CartCreateDto dto)
        {
            var response = await _services.AddItemToCartAsync(id,dto);

            return StatusCode(response.StatusCode, response);
        }
        
        // Get one car by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCartById(Guid id)
        {
            var response = await _services.GetCartAsync(id);
            
            return StatusCode(response.StatusCode, response);
        }
    }
}
