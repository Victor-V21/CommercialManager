using CommercialManager.API.Dtos.Common;
using CommercialManager.API.Dtos.Sales;
using CommercialManager.API.Dtos.Sales.Invoice;

namespace CommercialManager.API.Services.Interfaces
{
    public interface ISalesServices
    {
        Task<ResponseDto<SaleActionResponseDto>> CancelSaleAsync(Guid saleId);
        Task<ResponseDto<InvoiceDto>> GenerateInvoiceAsync(Guid saleId);
        Task<ResponseDto<SaleActionResponseDto>> ProcessSaleAsync(Guid userId);
    }
}
