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
                    Id = Guid.NewGuid() , 
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


            var detailsWithIds = await _context.SalesDetails
                .Where(sd => sd.SalesId == sale.Id)
                .ToListAsync();

            responseDto.Items = detailsWithIds.Select(detail => new SaleDetailDto
            {
                Id = detail.Id,  
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

        // Endpoint Cancelar
        public async Task<ResponseDto<SaleActionResponseDto>> CancelSaleAsync(Guid saleId)
        {
            var sale = await _context.Sales
                .Include(s => s.SalesDetail)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
                return new ResponseDto<SaleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = "La venta no ha sido encontrada",
                    Status = false
                };

            //Aqui hace el aumento del los productos del stocks
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var listItems = new List<SaleDetailDto>();

                    var saleDetails = await _context.SalesDetails
                       .Where(sd => sd.SalesId == sale.Id)
                       .Include(sd => sd.Product)
                       .ToListAsync();

                    foreach (var detail in saleDetails)
                    {
                        detail.Product.Stock += detail.Quantity;
                        _context.Products.Update(detail.Product);

                        listItems.Add(new SaleDetailDto
                        {
                            Id = detail.Id,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            UnitPrice = detail.UnitPrice,
                            ProductName = detail.Product.Name 
                        });
                    }

                    _context.SalesDetails.RemoveRange(sale.SalesDetail);
                    _context.Sales.Remove(sale);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ResponseDto<SaleActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "La venta a sido cancelada correctamente!",
                        Status = true,
                        Data = new SaleActionResponseDto
                        {
                            Id = sale.Id,
                            UserId = sale.UserId,
                            Date = sale.Date,
                            Total = sale.Total,
                            IsSuccess = true,
                            Message = "Compra cancelada exitosamente y productos almacenados",
                            Items = listItems 
                        }
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
