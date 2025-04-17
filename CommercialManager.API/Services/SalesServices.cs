using AutoMapper;
using CommercialManager.API.Database;
using CommercialManager.API.Services.Interfaces;

namespace CommercialManager.API.Services
{
    public class SalesServices : ISalesServices
    {

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

    }
}
