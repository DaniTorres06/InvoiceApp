using InvoiceBusiness.Interfaces;
using InvoiceData.Interfaces;
using InvoiceModel;
using InvoiceModel.DTO;
using InvoiceModel.ModelRsp;
using Microsoft.Extensions.Logging;

namespace InvoiceBusiness
{
    public class InvoicesBusiness : IInvoiceBusiness
    {
        private readonly ILogger<InvoicesBusiness> _logger;
        private readonly IInvoiceData _data;

        public InvoicesBusiness(ILogger<InvoicesBusiness> logger, IInvoiceData invoiceData)
        {
            _logger = logger;
            _data = invoiceData;
        }

        const int vDiscountZero = 0;
        const decimal vvDiscount = 0.05m;
        const decimal vIva = 0.019m;

        public async Task<Response> InvoiceAddAsync(InvoiceDTO vInvoiceDTO)
        {
            Response vObjRsp = new();

            try
            {
                vObjRsp = await ValidateInvoice(vInvoiceDTO);
                if (!vObjRsp.Status)
                {
                    vObjRsp.Status = false;
                    vObjRsp.Message = vObjRsp.Message;
                }
                else
                {
                    InvoiceHeader vInvHeader = new();

                    vInvHeader.DateInv = DateTime.UtcNow;
                    vInvHeader.DocumentClient = vInvoiceDTO.DocumentClient;
                    vInvHeader.Name = vInvoiceDTO.Name;
                    vInvHeader.LastName = vInvoiceDTO.LastName;
                    vInvHeader.AddressClient = vInvoiceDTO.AddressClient;
                    vInvHeader.PhoneClient = vInvoiceDTO.PhoneClient;

                    foreach (InvoiceBodyDTO item in vInvoiceDTO.InvoiceBodys)
                    {
                        InvoiceBody vInvBody = new();
                        vInvBody.ItemId = item.ItemId;
                        vInvBody.ItemQuantity = item.ItemQuantity;
                        vInvBody.Price = item.Price;

                        vInvBody.GrossValue = vInvBody.Price * vInvBody.ItemQuantity;
                        vInvBody.ValueDiscount = vInvBody.GrossValue >= 500000 ? (vInvBody.GrossValue * vvDiscount) : vDiscountZero;
                        vInvBody.IvaAmout = vInvBody.GrossValue * vIva;
                        vInvBody.TotalValue = (vInvBody.GrossValue - vInvBody.ValueDiscount) + vInvBody.IvaAmout;

                        vInvHeader.InvoiceBodys.Add(vInvBody);
                    }

                    //vInvHeader.InvoiceBodys.Add(vInvoiceDTO.InvoiceBodys);

                    vObjRsp = await _data.InvHeaderAddAsync(vInvHeader);
                }

                return vObjRsp;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                vObjRsp.Status = false;
                vObjRsp.Message = "Problemas en capa de negocio";
                return vObjRsp;
            }
        }

        private async Task<Response> ValidateInvoice(InvoiceDTO vInvoiceDTO)
        {
            Response vObjRsp = new();

            if (vInvoiceDTO.DocumentClient == 0)
            {
                vObjRsp.Status = false;
                vObjRsp.Message = "El documento del cliente es obligatorio";
                return vObjRsp;
            }
            else if (string.IsNullOrEmpty(vInvoiceDTO.Name))
            {
                vObjRsp.Status = false;
                vObjRsp.Message = "El nombre es requeridjo para la factura";
                return vObjRsp;
            }
            else if (string.IsNullOrEmpty(vInvoiceDTO.Name))
            {
                vObjRsp.Status = false;
                vObjRsp.Message = "El apellido es requeridjo para la factura";
                return vObjRsp;
            }
            else if (vInvoiceDTO.DateInv.Date < DateTime.Now.Date)
            {
                vObjRsp.Status = false;
                vObjRsp.Message = "La fecha de la factura debe ser igual o posterior a hoy";
                return vObjRsp;

            }
            else
            {
                vObjRsp.Status = true;
            }

            // Validaciones en BD
            Response vObjRspDb = await _data.InvValidateAsync(vInvoiceDTO.DocumentClient);

            if (vObjRspDb.Status)
            {
                vObjRsp.Status = false;
                vObjRsp.Message = vObjRspDb.Message;
                return vObjRsp;
            }

            if (vInvoiceDTO.InvoiceBodys.Count > 0)
            {
                foreach (InvoiceBodyDTO item in vInvoiceDTO.InvoiceBodys)
                {
                    if (item.ItemId == 0)
                    {
                        vObjRsp.Status = false;
                        vObjRsp.Message = "Se debe seleccionar un articulo";
                        return vObjRsp;
                    }
                    else if (item.ItemQuantity == 0)
                    {
                        vObjRsp.Status = false;
                        vObjRsp.Message = "La cantidad a llevar debe ser mayor a 0";
                        return vObjRsp;
                    }
                    else if (item.ItemQuantity > 50)
                    {
                        vObjRsp.Status = false;
                        vObjRsp.Message = "La cantidad a llevar no debe ser mayor a 50";
                        return vObjRsp;
                    }
                    else if (item.Price == 0)
                    {
                        vObjRsp.Status = false;
                        vObjRsp.Message = "Se debe digitar un precio";
                        return vObjRsp;
                    }
                    else
                    {
                        vObjRsp.Status = true;
                    }
                }
            }
            else
            {
                vObjRsp.Status = false;
                vObjRsp.Message = "Para crear la factura, es necesario incluir uno o más elementos en la lista de ítems."; // validar msj
                return vObjRsp;
            }

            return vObjRsp;
        }

        #region Nueva funcionalidad
        public async Task<RspRptInvoices> ReportInvAsync()
        {
            RspRptInvoices vObjRsp = new();
            try
            {
                vObjRsp = await _data.RspRptInvoices();
                return vObjRsp;
            }
            catch (Exception ex)
            {
                vObjRsp.Response.Status = false;
                vObjRsp.Response.Message = "Problemas en la capa de negocio " + ex;
                return vObjRsp;
            }
        }
        #endregion
    }
}
