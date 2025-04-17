using AutoMapper;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.CartsShoppings.Details;
using CommercialManager.API.Dtos.Categories;
using CommercialManager.API.Dtos.Products;
using CommercialManager.API.Dtos.Sales.Details;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Dtos.Users;
using CommercialManager.API.Dtos.Sales.Invoice;

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
            CreateMap<ProductEntity, ProductsDto>().ReverseMap();
            CreateMap<ProductEntity, ProductsActionResponseDto>().ReverseMap();

            // Mappers of UsersServices

            CreateMap<UsersCreateDto, UserEntity>();
            CreateMap<UserEntity, UsersDto>().ReverseMap();
            CreateMap<UserEntity, UsersActionResponseDto>().ReverseMap();

            // Mappers of CartShopping
            CreateMap<CartCreateDto, ShoppingCartEntity>().ReverseMap();
            CreateMap<CartDetailDto, ShoppingCartDetailEntity>().ReverseMap();
            CreateMap<CartDto, CartCreateDto>().ReverseMap();

            // Mappers of SalesService

            //Details Sales
            CreateMap<SalesDetailEntity, SaleDetailDto>().ReverseMap();
            CreateMap<SaleDetailCreateDto, SalesDetailEntity>();

            //Sales 
            CreateMap<SalesEntity, SalesDto>().ReverseMap();
            CreateMap<SaleCreateDto, SalesEntity>();
            CreateMap<SalesEntity, InvoiceDto>();
            //billing sales details
            CreateMap<SalesDetailEntity, InvoiceItemDto>();
            CreateMap<SalesEntity, SaleActionResponseDto>();

            // Others Mappers profiles ...
        }
    }
}
