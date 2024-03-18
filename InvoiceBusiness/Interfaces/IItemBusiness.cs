using InvoiceModel.ModelRsp;

namespace InvoiceBusiness.Interfaces
{
    public interface IItemBusiness
    {
        Task<RspItems> RspItems();
    }
}