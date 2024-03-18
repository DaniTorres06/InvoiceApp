using InvoiceData.Interfaces;
using InvoiceModel.DTO;
using InvoiceModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InvoiceModel.ModelRsp;
using InvoiceBusiness.Interfaces;

namespace InvoiceBusiness
{
    public class ItemBusiness : IItemBusiness
    {
        private readonly ILogger<ItemBusiness> _logger;
        private readonly IItemData _data;

        public ItemBusiness(ILogger<ItemBusiness> logger, IItemData data)
        {
            _logger = logger;
            _data = data;
        }

        public async Task<RspItems> RspItems()
        {
            RspItems vObjRsp = new();

            try
            {
                vObjRsp = await _data.RspItems();

                return vObjRsp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                vObjRsp.Response.Status = false;
                vObjRsp.Response.Message = "Problemas en capa de negocio";
                return vObjRsp;
            }
        }

    }
}
