using AutoMapper;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos;
using CommercialManager.API.Dtos.Categories;

namespace CommercialManager.API.Services
{

    public class CategoriesServices
    {

        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;

        public CategoriesServices(CommercialDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // ------ Create a category -----
        public async Task<ResponseDto<CategoryCreateDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var categoryEntity = _mapper.Map<CategoryEntity>(dto);

            categoryEntity.Id = Guid.NewGuid();

            _context.Add(categoryEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<CategoryCreateDto>
            {
                StatusCode = 200,
                Status = true,
                Message = "Registro creado correctamente",
                Data = _mapper.Map<CategoryCreateDto>(categoryEntity)
            };
        }

    }
}
