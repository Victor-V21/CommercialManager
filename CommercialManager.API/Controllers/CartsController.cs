using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
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
        public async Task<ActionResult<ResponseDto<CartActionResponseDto>>> AddItems(Guid id, [FromBody] CartCreateDto dto)
        {
            var response = await _services.AddItemToCartAsync(id, dto);

            return StatusCode(response.StatusCode, response);
        }

        // Get one car by id
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCartById(Guid id)
        {
            var response = await _services.GetCartAsync(id);

            return StatusCode(response.StatusCode, response);
        }

        // Remove items with quantity (default in negative, to rest)

        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDto<CartActionResponseDto>>> PutItems(Guid id, [FromBody] CartEditDto dto)
        {
            var response = await _services.RemoveItemsToCartAsync(id, dto);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDto<CartActionResponseDto>>> DeleteCartById(Guid id)
        {
            var response = await _services.DeleteCart(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
