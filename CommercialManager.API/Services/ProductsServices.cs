using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Products;
using CommercialManager.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public ProductsServices(CommercialDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }

        //----Products Creation----

        public async Task<ResponseDto<ProductsActionResponseDto>> CreateAsync(ProductsCreateDto dto)
        {
            var productEntity = _mapper.Map<ProductEntity>(dto);
                _context.Products.Add(productEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<ProductsActionResponseDto>
            {
                StatusCode = Constants.HttpStatusCode.CREATED,
                Status = true,
                Message = "Registro creado correctamente",
                Data = _mapper.Map<ProductsActionResponseDto>(productEntity)
            };

        }
        // ----To edit products----
        public async Task<ResponseDto<PaginationDto<List<ProductsDto>>>> GetListAsync(
            string searchTerm = "", int page = 1, int pageSize = 0
            )
        {
            pageSize = pageSize == 0 ? PAGE_SIZE : pageSize;

            int startIndex = (page - 1) * pageSize;

            IQueryable<ProductEntity> productsQuery = _context.Products;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery
                    .Where(x => (x.Name + " " + x.Description).Contains(searchTerm));
            }

            int totalRows = productsQuery.Count();

            var productsEntity = await productsQuery
                .OrderBy(x => x.Name).Skip(startIndex).Take(pageSize).ToListAsync();

            var productsDto = _mapper.Map<List<ProductsDto>>(productsEntity);

            return new ResponseDto<PaginationDto<List<ProductsDto>>>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro encontrado correctamente",
                Data = new PaginationDto<List<ProductsDto>>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalRows,
                    TotalPages = (int)Math.Ceiling((double)totalRows / pageSize),
                    HasPreviousPage = page > 1,
                    HasNextPage =
                 startIndex + pageSize < PAGE_SIZE_LIMIT && page < (int)Math.Ceiling((double)totalRows / pageSize),
                    Items = productsDto


                }
            };

        }
        // ----To edit products----
        public async Task<ResponseDto<ProductsActionResponseDto>> EditAsync(ProductsEditDto dto, Guid id)
        {
            var productEntity = await _context.Products.FindAsync(id);

            if (productEntity == null)
            {
                return new ResponseDto<ProductsActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            _mapper.Map<ProductsEditDto, ProductEntity>(dto, productEntity);

            _context.Products.Update(productEntity);

            await _context.SaveChangesAsync();

            return new ResponseDto<ProductsActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Editado Correctamente",
                Data = _mapper.Map<ProductsActionResponseDto>(productEntity)
            };

        }

        // ----Products Delete----

        public async Task<ResponseDto<ProductsActionResponseDto>> DeleteAsync(Guid id)
        {
            var productEntity = await _context.Products.FindAsync(id);

            if (productEntity == null)
            {
                return new ResponseDto<ProductsActionResponseDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "No se encontro el registro",
                    Data = null
                };
            }

            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();

            return new ResponseDto<ProductsActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registro Eliminado Correctamente",
                Data = _mapper.Map<ProductsActionResponseDto>(productEntity)
            };
        }
    }
}
