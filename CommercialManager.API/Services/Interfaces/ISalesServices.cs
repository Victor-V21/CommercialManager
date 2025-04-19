using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ISalesServices
    {
        Task<ResponseDto<SaleActionResponseDto>> ProcessSaleAsync(Guid userId);
    }
}
