using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.CartsShoppings;
using CommercialManager.API.Dtos.CartsShoppings.Details;
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
                                Message = "Registro no encontrado" // el usuario no existe
                            };
                        }

                        //Se añade el cart en memoria
                        cartEntity = _mapper.Map<ShoppingCartEntity>(dto);
                        cartEntity.UserId = id;


                        _context.ShoppingCarts.Add(cartEntity);
                        //await _context.SaveChangesAsync();
                    }

                    //Verifica que contenga Items para agregar
                    if (dto.Items is null)
                    {
                        return new ResponseDto<CartDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = "Items no encontrado"
                        };
                    }

                    // Validar que todos los productos existen en la base de datos
                    var productIds = dto.Items.Select(x => x.ProductId).ToList();

                    var existingProductIds = await _context.Products
                        .Where(x => productIds.Contains(x.Id)) 
                        //x => x.id = id
                        .Select(x => x.Id)
                        .ToListAsync();

                    var invalidProducts = productIds.Except(existingProductIds).ToList();

                    // Si hay productos invalidos en la lista, se enviaran
                    if (invalidProducts.Any())
                    {
                        return new ResponseDto<CartDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = $"Los siguietes productos no existen: {string.Join(", ", invalidProducts)}"
                        };
                    }


                    // Si existe el producto en cart le suma la cantidad, si no, crea un registro
                    foreach (var item in dto.Items)
                    {
                        var product = await _context.Products
                            .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                        var price = product.Price;
                        var discount = product.Discount;
                        var quantity = item.Quantity;
                        var subtotal = (price - discount * price) * quantity;

                        var existingDetail = await _context.ShoppingCartDetails
                            .FirstOrDefaultAsync(x => x.ShoppingCartId == cartEntity.UserId && x.ProductId == item.ProductId);

                        if (existingDetail != null)
                        {
                            // Actualiza cantidad y subtotal
                            existingDetail.Quantity += quantity;
                            existingDetail.Subtotal = (decimal)(price - discount * price) * existingDetail.Quantity;
                            //                          ^^ Esto me daba un error
                            _context.ShoppingCartDetails.Update(existingDetail);
                        }
                        else
                        {
                            var newDetail = new ShoppingCartDetailEntity
                            {
                                Id = Guid.NewGuid(),
                                ProductId = item.ProductId,
                                Quantity = quantity,
                                ShoppingCartId = cartEntity.UserId,
                                Subtotal = (decimal)subtotal
                            };
                            _context.ShoppingCartDetails.Add(newDetail);
                        }
                    }

                    // Guarda los datos modificados o añadidos para luego continuar con la tabla madre
                    await _context.SaveChangesAsync();

                    // Calcula el total de los subtotales de los detalles del carrito
                    var totalCartAmount = await _context.ShoppingCartDetails
                        .Where(x => x.ShoppingCartId == id)
                        .SumAsync(x => x.Subtotal);

                    cartEntity.TotalAmount = totalCartAmount;

                    //Generando response para retornar los valores
                    var response = _mapper.Map<CartDto>(dto);

                    response.UserId = id;

                    response.TotalItems = await _context.ShoppingCartDetails
                        .Where(x => x.ShoppingCartId == id)
                        .SumAsync(x => x.Quantity);

                    // Actualiza la cantidad total de productos que hay en el carrito
                    cartEntity.TotalItems = response.TotalItems;

                    _context.ShoppingCarts.Update(cartEntity);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return new ResponseDto<CartDto>
                    {
                        StatusCode = HttpStatusCode.CREATED,
                        Status = true,
                        Message = "Registros Añadidos correctamente",
                        Data = response
                    };
                }
                catch (SqliteException e)
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
        
        //Crear Get de un carrito por el id

        public async Task<ResponseDto<CartDto>> GetCartAsync(Guid id)
        {
            var cartEntity = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == id);

            if (cartEntity is null)
            {
                return new ResponseDto<CartDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            var cartDetailsEntity = await _context.ShoppingCartDetails.Where(x => x.ShoppingCartId == id).ToListAsync();

            var response = _mapper.Map<CartDto>(cartEntity);
            response.Items = _mapper.Map<List<CartDetailDto>>(cartDetailsEntity);

            return new ResponseDto<CartDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros encontrados correctamente",
                Data = response
            };
        }

        //Eliminar productos de un Carrito

        public async Task<ResponseDto<CartDto>> RemoveItemsToCartAsync(Guid id, CartEditDto dto)
        {
            var cartEntity = await _context.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == id);

            if (cartEntity is null)
            {
                return new ResponseDto<CartDto>
                {
                    StatusCode = HttpStatusCode.BAD_REQUEST,
                    Status = false,
                    Message = "Registro no encontrado"
                };
            }

            var productsInCart = await _context.ShoppingCartDetails
                .Where(x => x.ShoppingCartId == id).ToListAsync();

            var idsProdcuts = productsInCart.Select(x => x.ProductId).ToList();

            var itemsToRemove = new List<ShoppingCartDetailEntity>();
            var itemsToUpdate = new List<ShoppingCartDetailEntity>();

            foreach (var item in dto.Items)
            {
                if (idsProdcuts.Contains(item.ProductId))
                {
                    var detailExisting = productsInCart.FirstOrDefault(x => x.ProductId == item.ProductId);

                    if (detailExisting == null) continue;

                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (item.Quantity <= detailExisting.Quantity)
                    {
                        detailExisting.Quantity -= item.Quantity;
                    }
                    else
                    {
                        return new ResponseDto<CartDto>
                        {
                            StatusCode = HttpStatusCode.BAD_REQUEST,
                            Status = false,
                            Message = $"En productId: {item.ProductId}, solo hay {detailExisting.Quantity} cantidades"
                        };
                    }

                    if (detailExisting.Quantity == 0)
                    {
                        itemsToRemove.Add(detailExisting);
                    }
                    else
                    {
                        var price = product.Price;
                        var discount = product.Discount;
                        var subtotal = (price - discount * price) * detailExisting.Quantity;

                        detailExisting.Subtotal = (decimal)subtotal;
                        itemsToUpdate.Add(detailExisting);
                    }
                }
            }

            // Aplicar cambios
            if (itemsToRemove.Any())
                _context.ShoppingCartDetails.RemoveRange(itemsToRemove);

            if (itemsToUpdate.Any())
                _context.ShoppingCartDetails.UpdateRange(itemsToUpdate);

            // Recalcular totales sobre SOLO los que quedan
            var remainingProducts = productsInCart.Except(itemsToRemove).ToList();

            var totalCartAmount = remainingProducts.Sum(x => x.Subtotal);
            var totalItems = remainingProducts.Sum(x => x.Quantity);

            cartEntity.TotalAmount = totalCartAmount;
            cartEntity.TotalItems = totalItems;

            _context.ShoppingCarts.Update(cartEntity);

            await _context.SaveChangesAsync();

            var response = new CartDto
            {
                UserId = id,
                CreateDate = dto.CreateDate,
                TotalAmount = totalCartAmount,
                TotalItems = totalItems,
                Items = _mapper.Map<List<CartDetailDto>>(remainingProducts)
            };

            return new ResponseDto<CartDto>
            {
                StatusCode = HttpStatusCode.OK,
                Status = true,
                Message = "Registros Editados Correctamente",
                Data = response
            };
        }

    }
}
