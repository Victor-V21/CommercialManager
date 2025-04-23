using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;

namespace CommercialManager.API.Services.Interfaces
{
    public interface IShoppingCartsServices
    {
        Task<ResponseDto<CartActionResponseDto>> AddItemToCartAsync(Guid id, CartCreateDto dto);
        Task<ResponseDto<CartActionResponseDto>> DeleteCart(Guid id);
        Task<ResponseDto<CartDto>> GetCartAsync(Guid id);
        Task<ResponseDto<CartDto>> RemoveItemsToCartAsync(Guid id, CartEditDto dto);
    }
}
