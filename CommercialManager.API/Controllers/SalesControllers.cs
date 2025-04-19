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

        //Cancelacion
        [HttpDelete("{saleId}")]
        public async Task<ActionResult<ResponseDto<SaleActionResponseDto>>> CancelSale(Guid saleId)
        {
            try
            {
                var result = await _services.CancelSaleAsync(saleId);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.INTERNAL_SERVER_ERROR, new ResponseDto<SaleActionResponseDto>
                {
                    StatusCode = HttpStatusCode.INTERNAL_SERVER_ERROR,
                    Message = $"Error interno: {ex.Message}",
                    Status = false
                });
            }
        }

    }
}
