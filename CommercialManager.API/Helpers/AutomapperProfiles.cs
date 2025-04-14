using AutoMapper;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Categories;

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


            // Others Mappers ...

        }
    }
}
