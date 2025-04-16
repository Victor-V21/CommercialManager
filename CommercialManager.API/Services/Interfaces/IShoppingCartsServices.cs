using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;

namespace CommercialManager.API.Services.Interfaces
{
    public interface IShoppingCartsServices
    {
        Task<ResponseDto<CartDto>> AddItemToCartAsync(Guid id, CartCreateDto dto);
    }
}
