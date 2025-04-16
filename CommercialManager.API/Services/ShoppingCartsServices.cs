using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CommercialManager.API.Services
{
    public class ShoppingCartsServices : IShoppingCartsServices
    {

        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public ShoppingCartsServices(CommercialDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }


        // Add items to a new cart or an existent cart, whit user´s id
        public async Task<ResponseDto<CartDto>> AddItemToCartAsync(Guid id, CartCreateDto dto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var cartEntity = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == id);

                    // Verifica si ya existe un cart
                    if (cartEntity is null)
                    {

                        var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

                        // Verifica si existe el usuario el cual quiere añadirle el cart
                        if (userEntity is null)
                        {
                            return new ResponseDto<CartDto>
                            {
                                StatusCode = HttpStatusCode.BAD_REQUEST,
                                Status = false,
                                Message = "Registro no encontrado"
                            };
                        }

                        //Se añade el cart en memoria
                        cartEntity = _mapper.Map<ShoppingCartEntity>(dto);
                        cartEntity.UserId = id;

                        _context.ShoppingCarts.Add(cartEntity);
                        await _context.SaveChangesAsync();
                    }

                    //Verifica que contenga Items para agregar
                    if (dto.Items is null)
                    {
                        return new ResponseDto<CartDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = "ITEMS no encontrado"
                        };
                    }

                    // Pasa los items ingresados a una variable
                    var cartDetails = _mapper.Map<List<ShoppingCartDetailEntity>>(dto.Items);
                    // Declarar sus variables independienemente
                    
                    cartDetails = cartDetails.Select(x => new ShoppingCartDetailEntity
                    {
                        Id = Guid.NewGuid(),
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        ShoppingCartId = id
                    }).ToList();

                    _context.ShoppingCartDetails.AddRange(cartDetails);

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    var response = _mapper.Map<CartDto>(dto);

                    response.TotalItems = await _context.ShoppingCartDetails.Where(x => x.ShoppingCartId==id).CountAsync();

                    return new ResponseDto<CartDto>
                    {
                        StatusCode = HttpStatusCode.CREATED,
                        Status = true,
                        Message = "Registros Añadidos correctamente",
                        Data = response
                    };
                }
                catch(SqliteException e)
                {
                    transaction.Rollback();
                    return new ResponseDto<CartDto>
                    {
                        StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                        Status = false,
                        Message = "Error interno en el servidor : " + e.Message
                    };
                }
            }
        }

    }
}
