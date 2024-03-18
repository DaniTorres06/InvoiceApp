using InvoiceModel.DTO;
using InvoiceModel.ModelRsp;

namespace InvoiceBusiness.Interfaces
{
    public interface IInvoiceBusiness
    {
        Task<Response> InvoiceAddAsync(InvoiceDTO vInvoiceDTO);
        Task<RspRptInvoices> ReportInvAsync();
    }
}