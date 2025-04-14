using System.Net;
using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Products;
using CommercialManager.API.Services.Interfaces;

namespace CommercialManager.API.Services
{
    public class ProductsServices : IProductsServices
    {
        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;

        public ProductsServices(CommercialDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
    }
}
