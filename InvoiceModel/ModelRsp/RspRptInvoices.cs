using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel.ModelRsp
{
    public class RspRptInvoices
    {
        public List<RptInvoices>? LstInvoces { get; set; }
        public Response Response { get; set; }

        public RspRptInvoices() 
        {
            LstInvoces = new();
            Response = new Response();
        }

    }
}
