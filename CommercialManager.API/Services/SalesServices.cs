using AutoMapper;
using CommercialManager.API.Database;
using CommercialManager.API.Services.Interfaces;

namespace CommercialManager.API.Services
{
    public class SalesServices : ISalesServices
    {
<<<<<<< HEAD
        // Endpoint para realizar una compra, todos los datos del carrito se pasan a la tabla de ventas (con sus details), y se debe eliminar el stock

        // Endpoint Cancelar

        // Endpoint de imprimir factura con referencia a nombres de la tabla de productos.
=======
        private readonly CommercialDbContext _context;
        private readonly IMapper _mapper;
        private readonly int PAGE_SIZE;
        private readonly int PAGE_SIZE_LIMIT;

        public SalesServices(CommercialDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            PAGE_SIZE = configuration.GetValue<int>("PageSize");
            PAGE_SIZE_LIMIT = configuration.GetValue<int>("PageSizeLimit");
        }
>>>>>>> c73bc2d (Creation of sales dtos)
    }
}
