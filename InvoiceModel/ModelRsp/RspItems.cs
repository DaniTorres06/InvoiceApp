using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceModel.ModelRsp
{
    public class RspItems
    {
        public Response? Response { get; set; }
        public List<Items>? vLstItems { get; set; }

        public RspItems() 
        {
            Response = new Response();
            vLstItems = new List<Items>();
        }

    }
}
