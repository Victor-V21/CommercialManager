using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ICategoriesServices
    {
        Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CategoryCreateDto dto);
        Task<ResponseDto<CategoryActionResponseDto>> DeleteByIdAsync(Guid id);
        Task<ResponseDto<CategoryActionResponseDto>> EditByIdAsync(Guid id, CategoryEditDto dto);
        Task<ResponseDto<PaginationDto<List<CategoryDto>>>> GetListAsync(string searchTerm = "", int page = 1, int pageSize = 0);
        Task<ResponseDto<CategoryActionResponseDto>> GetOneByIdAsync(Guid id);
    }
}
