using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ICategoriesServices
    {
        Task<ResponseDto<CategoryCreateDto>> CreateCategoryAsync(CategoryCreateDto dto);
    }
}
