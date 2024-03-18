using InvoiceModel.ModelRsp;

namespace InvoiceData.Interfaces
{
    public interface IItemData
    {
        Task<RspItems> RspItems();
    }
}