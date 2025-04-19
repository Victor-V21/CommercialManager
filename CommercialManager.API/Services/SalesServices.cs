using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Dtos.Sales.Details;
using CommercialManager.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CommercialManager.API.Services
{
    public class SalesServices : ISalesServices
    {

        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;

        public SalesServices(CommercialDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;

        }
        // Endpoint para realizar una compra, todos los datos del carrito se pasan a
        // la tabla de ventas (con sus details), y se debe eliminar el stock
        public async Task<ResponseDto<SaleActionResponseDto>> ProcessSaleAsync(Guid userId)
        {
            //Realizacion de la obtencion del carrito de la base de datos 
            var cart = await _context.ShoppingCarts
                .Include(c => c.Details)
                .ThenInclude(cd => cd.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Details.Any()) //Validar que usuario tenga el carrito
                return new ResponseDto<SaleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = "No se logro encontrar el carrito",
                    Status = false
                };

            // En este paso haremos la validacion de stock que tenga el producto, su precio en si 
            foreach (var item in cart.Details)
            {
                if (item.Product.Stock < item.Quantity)
                    return new ResponseDto<SaleActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Message = $"Stock a sido insuficiente para {item.Product.Name}",
                        Status = false
                    };

                if (item.Product.Price == null)
                    return new ResponseDto<SaleActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.BAD_REQUEST,
                        Message = $"El producto {item.Product.Name} no tiene precio dado",
                        Status = false
                    };
            }

            // Creamos lo que es la venta llamando al salesEntity 
            var sale = new SalesEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Date = DateTime.UtcNow,
                Total = (double)cart.Details.Sum(item => item.Quantity * item.Product.Price!.Value),
                SalesDetail = cart.Details.Select(item => new SalesDetailEntity
                {
                    Id = Guid.NewGuid() , //Tengo un error en esta parte me devuelve ceros 
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = (double)item.Product.Price!.Value 
                }).ToList()
            };

            // Creamos el foreach para limpiar los datos del carrito y reduccir el stock 
            foreach (var item in cart.Details)
            {
                item.Product.Stock -= item.Quantity;
                _context.Products.Update(item.Product);
            }
            _context.ShoppingCartDetails.RemoveRange(cart.Details);

           
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            var responseDto = _mapper.Map<SaleActionResponseDto>(sale);
            responseDto.Message = "Gracias por su compra, vuelva pronto!";
            responseDto.IsSuccess = true;
            responseDto.Items = sale.SalesDetail.Select(detail => new SaleDetailDto
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice 
            }).ToList();

            return new ResponseDto<SaleActionResponseDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Venta realizada exitosamente!",
                Status = true,
                Data = responseDto
            };
        }

    }
}
