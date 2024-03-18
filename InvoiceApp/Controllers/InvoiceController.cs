using InvoiceBusiness.Interfaces;
using InvoiceModel.DTO;
using InvoiceModel.ModelRsp;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ILogger<InvoiceController> _logger;
        private readonly IInvoiceBusiness _service;

        public InvoiceController(ILogger<InvoiceController> logger, IInvoiceBusiness invoiceBusiness)
        {
            _logger = logger;
            _service = invoiceBusiness;
        }

        [HttpPost("InvoiceAddAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> InvHeaderAddAsync([FromBody] InvoiceDTO vInvoiceDTO)
        {
            try
            {
                var response = await _service.InvoiceAddAsync(vInvoiceDTO);

                if (response is null)
                    return BadRequest();
                if (!response.Status)
                    return BadRequest(response);
                else
                    return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        #region Nueva funcionalidad

        [HttpGet("ReportInvAsync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RspRptInvoices))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RspRptInvoices>> ReportInvAsync()
        {
            try
            {
                var response = await _service.ReportInvAsync();

                if (response is null)
                    return BadRequest();
                if (!response.Response.Status)
                    return BadRequest(response);
                else
                    return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest();
            }
        }

        #endregion

    }
}
