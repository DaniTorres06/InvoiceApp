using InvoiceBusiness.Interfaces;
using InvoiceModel.ModelRsp;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceApp.Controllers
{
    public class ItemController : Controller
    {
        private readonly ILogger<ItemController> _logger;
        private readonly IItemBusiness _service;

        public ItemController(ILogger<ItemController> logger, IItemBusiness invoiceBusiness)
        {
            _logger = logger;
            _service = invoiceBusiness;
        }

        [HttpGet("RspItems")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RspItems))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RspItems>> RspItems()
        {
            try
            {
                var response = await _service.RspItems();

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

    }
}
