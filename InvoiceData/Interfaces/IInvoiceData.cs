using InvoiceModel;
using InvoiceModel.ModelRsp;

namespace InvoiceData.Interfaces
{
    public interface IInvoiceData
    {
        Task<Response> InvBodyAddAsync(InvoiceBody vInvoiceBody);
        Task<Response> InvHeaderAddAsync(InvoiceHeader vInvoiceHeader);
        Task<Response> InvValidateAsync(Int64 vDocumentClient);
        Task<RspRptInvoices> RspRptInvoices();
    }
}