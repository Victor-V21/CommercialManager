using AutoMapper;
using CommercialManager.API.Constants;
using CommercialManager.API.Database;
using CommercialManager.API.Database.Entities;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Dtos.Sales.Details;
using CommercialManager.API.Dtos.Sales.Invoice;
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

                if (item.Product.Price == null) //Valida por si el precio del producto es null
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
                Total = (double)
                    cart.Details.Sum(item => item.Subtotal),
                SalesDetail = cart.Details.Select(item => new SalesDetailEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.Product.Id,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Discount = item.Product.Discount,
                    UnitPrice = item.Product.Price.Value 
                }).ToList()
            };

            // Creamos el foreach para limpiar los datos del carrito y reduccir el stock 
            foreach (var item in cart.Details)
            {
                item.Product.Stock -= item.Quantity;
                _context.Products.Update(item.Product);
            }

            _context.ShoppingCartDetails.RemoveRange(cart.Details);
             _context.Sales.Add(sale);
            _context.ShoppingCarts.Remove(cart);

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
                ProductName = detail.Product.Name,
                Discount = detail.Product.Discount,
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
                            ProductName = detail.Product.Name,
                            Quantity = detail.Quantity,
                            Discount = detail.Discount,
                            UnitPrice = detail.UnitPrice,
                            
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
                    //throw;
                    return new ResponseDto<SaleActionResponseDto>
                    {
                        StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                        Message = "Error interno del servidor",
                        Status = false
                    };
                }
            }
        }

        //Facturacion
        public async Task<ResponseDto<InvoiceDto>> GenerateInvoiceAsync(Guid saleId)
        {
            // Aqui vamos a obtener lo que son los datos del usuario, el producto y los detallas de la venta 
            var sale = await _context.Sales
                .Include(s => s.SalesDetail)
                    .ThenInclude(sd => sd.Product)
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
                return new ResponseDto<InvoiceDto>
                {
                    StatusCode = HttpStatusCode.NOT_FOUND,
                    Message = "La venta no ha sido encontrada!",
                    Status = false
                };

            //Aqui hacemos el mapeo de los detalles de la compra 
            var invoiceItems = sale.SalesDetail.Select(detail => new InvoiceItemDto
            {
                ProductId = detail.ProductId,
                ProductName = detail.Product.Name,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                Discount = (decimal)detail.Discount,
                Subtotal = (decimal)(detail.Quantity * detail.UnitPrice),
                Total = (decimal)(detail.UnitPrice - (detail.Discount * detail.UnitPrice)) * detail.Quantity
            }).ToList();

            //Aqui hacemos el ingreso de los datos del cliente, el del producto y por ultimo el total a pagar
            var invoice = new InvoiceDto
            {
                SaleId = sale.Id,
                Date = sale.Date,
                ClientName = $"{sale.User.FirstName} {sale.User.LastName}", //unimos el nombre y apellido 
                ClientDNI = sale.User.DNI,
                TotalAmount = invoiceItems.Sum(item => item.Total),
                Items = invoiceItems,
            };

            return new ResponseDto<InvoiceDto>
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Facturacion, Gracias por si compra!",
                Status = true,
                Data = invoice
            };
        }
    }
}
