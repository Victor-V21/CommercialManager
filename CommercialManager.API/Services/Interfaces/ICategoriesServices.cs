using CommercialManager.API.Dtos;
using CommercialManager.API.Dtos.Categories;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ICategoriesServices
    {
        Task<ResponseDto<CategoryCreateDto>> CreateCategoryAsync(CategoryCreateDto dto);
    }
}
