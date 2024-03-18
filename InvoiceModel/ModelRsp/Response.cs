namespace InvoiceModel.ModelRsp
{
    public class Response
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public bool Status { get; set; }

        public Response()
        {
            Message = string.Empty;
            Code = string.Empty;
            Status = false;
        }
    }
}
