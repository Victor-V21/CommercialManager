using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Products;

namespace CommercialManager.API.Services.Interfaces
{
    public interface IProductsServices
    {
        Task<ResponseDto<ProductsActionResponseDto>> CreateAsync(ProductsCreateDto dto);
        Task<ResponseDto<ProductsActionResponseDto>> DeleteAsync(Guid id);
        Task<ResponseDto<ProductsActionResponseDto>> EditAsync(ProductsEditDto dto, Guid id);
        Task<ResponseDto<PaginationDto<List<ProductsDto>>>> GetListAsync(string searchTerm = "", int page = 1, int pageSize = 0);
    }
}
