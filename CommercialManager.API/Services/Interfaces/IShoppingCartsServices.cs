using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;

namespace CommercialManager.API.Services.Interfaces
{
    public interface IShoppingCartsServices
    {
        Task<ResponseDto<CartDto>> AddItemToCartAsync(Guid id, CartCreateDto dto);
        Task<ResponseDto<CartDto>> GetCartAsync(Guid id);
        Task<ResponseDto<CartDto>> RemoveItemsToCartAsync(Guid id, CartEditDto dto);
    }
}
