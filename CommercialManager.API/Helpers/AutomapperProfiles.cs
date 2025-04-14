using AutoMapper;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Products;

namespace CommercialManager.API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {

            // Mappers of CategoryServices

            CreateMap<CategoryCreateDto, CategoryEntity>();

            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();

            CreateMap<CategoryEntity, CategoryActionResponseDto>().ReverseMap();

            // Mappers of ProductsServices

            CreateMap<ProductsCreateDto, ProductEntity>();
            CreateMap<ProductsEditDto, ProductEntity>();
            CreateMap<ProductEntity, ProductsDto>();
            CreateMap<ProductEntity, ProductsActionResponseDto>();


            // Others Mappers ...

        }
    }
}
