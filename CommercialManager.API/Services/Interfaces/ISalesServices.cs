using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Dtos.Sales.Invoice;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ISalesServices
    {
        Task<ResponseDto<SaleActionResponseDto>> CancelSaleAsync(Guid saleId);
        Task<ResponseDto<InvoiceDto>> GenerateInvoiceAsync(Guid saleId);
        Task<ResponseDto<PaginationDto<List<InvoiceDto>>>> GetSalesByUserIdAsync(Guid userId, int page = 1, int pageSize = 0);
        Task<ResponseDto<SaleActionResponseDto>> ProcessSaleAsync(Guid userId);
    }
}
