using CommercialManager.API.Constants;
using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Services;
using CommercialManager.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommercialManager.API.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesControllers : Controller
    {
        private readonly ISalesServices _services;

        public SalesControllers(ISalesServices services)
        {
            _services = services;
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<ResponseDto<SaleActionResponseDto>>> ProcessSale(Guid userId)
        {
            var result = await _services.ProcessSaleAsync(userId);
            return StatusCode(result.StatusCode, result);
        }

    }
}
