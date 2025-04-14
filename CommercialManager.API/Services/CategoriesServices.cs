using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Services
{

    public class CategoriesServices : ICategoriesServices
    {

        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public CategoriesServices(CommercialDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        // ------ Create a category -----
        public async Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CategoryCreateDto dto)
        {
            var categoryEntity = _mapper.Map<CategoryEntity>(dto);

            categoryEntity.Id = Guid.NewGuid();

            _context.Categories.Add(categoryEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<CategoryDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro creado correctamente",
                Data = _mapper.Map<CategoryDto>(categoryEntity)
            };
        }

        // ----- Read Categories List -----
        public async Task<ResponseDto<PaginationDto<List<CategoryDto>>>> GetListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? PAGE_SIZE: pageSize; 

            int startIndex = (page - 1) * pageSize;

            IQueryable<CategoryEntity> categoryQuery = _context.Categories;

            if(!string.IsNullOrEmpty(searchTerm))
            {
                categoryQuery = categoryQuery
                    .Where(x => (x.Name + " " + x.Description).Contains(searchTerm));
            }

            int totalRows = categoryQuery.Count();

            var categoryEntity = await categoryQuery
                .OrderBy(x => x.Name).Skip(startIndex).Take(pageSize).ToListAsync();

            var countryDto = _mapper.Map<List<CategoryDto>>(categoryEntity);

           
            return new ResponseDto<PaginationDto<List<CategoryDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = new PaginationDto<List<CategoryDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page>1,
                    HasNextPage = 
                        startIndex + pageSize < PAGE_SIZE_LIMIT && page < (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = countryDto
                }
            };
        }

        // ----- Read One by id -----
        public async Task<ResponseDto<CategoryActionResponseDto>> GetOneByIdAsync(Guid id)
        {
           var response = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (response == null)
            {
                return new ResponseDto<CategoryActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            return new ResponseDto<CategoryActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Encontrado Correctamente",
                Data = _mapper.Map<CategoryActionResponseDto>(response)
            };
        }

        // -----  Modify One by id -----
        public async Task<ResponseDto<CategoryActionResponseDto>> EditByIdAsync(Guid id, CategoryEditDto dto)
        {
            var categoryEntity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(categoryEntity == null)
            {
                return new ResponseDto<CategoryActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            _mapper.Map<CategoryEditDto, CategoryEntity>(dto, categoryEntity);

            _context.Categories.Update(categoryEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<CategoryActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Editado Correctamente",
                Data = _mapper.Map<CategoryActionResponseDto>(categoryEntity)
            };
        }

        // ----- Delete Category by id -----
        public async Task<ResponseDto<CategoryActionResponseDto>> DeleteByIdAsync(Guid id)
        {
            var categoryEntity = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (categoryEntity == null)
            {
                return new ResponseDto<CategoryActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            _context.Categories.Remove(categoryEntity);
            await _context.SaveChangesAsync();
            
            return new ResponseDto<CategoryActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Eliminado Correctamente",
                Data = _mapper.Map<CategoryActionResponseDto>(categoryEntity)
            };
        }
    }
}
