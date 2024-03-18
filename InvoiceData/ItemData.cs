using InvoiceData.Interfaces;
using InvoiceModel;
using InvoiceModel.ModelRsp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InvoiceData
{
    public class ItemData : IItemData
    {
        private readonly ILogger<ItemData> _logger;
        private readonly IConfiguration _config;

        public ItemData(ILogger<ItemData> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        private const string InvoiceGetItems = "invoice_items";

        public async Task<RspItems> RspItems()
        {
            RspItems vObjRsp = new();

            SqlConnection conn = new(_config["ConnectionStrings:SqlServer"]);
            try
            {
                SqlCommand StoreProc_enc = new(InvoiceGetItems, conn);
                StoreProc_enc.CommandType = CommandType.StoredProcedure;

                //StoreProc_enc.Parameters.Add("@DateInv", SqlDbType.Date).Value = vInvoiceHeader.DateInv;

                conn.Open();
                using SqlDataReader reader = await StoreProc_enc.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["HasErrors"]) == 0)
                    {
                        Items vObjItems = new();
                        vObjItems.IdItem = reader["IdItem"] != DBNull.Value ? Convert.ToInt32(reader["IdItem"].ToString()) : 0;
                        vObjItems.DescItem = reader["DescItem"] != DBNull.Value ? reader["DescItem"].ToString() : string.Empty;

                        vObjRsp.vLstItems?.Add(vObjItems);
                        vObjRsp.Response.Status = true;
                        vObjRsp.Response.Message = "Se encontraron registros.";

                    }
                    else
                    {
                        vObjRsp.Response.Status = false;
                        vObjRsp.Response.Message = "No se encontraron registros para mostrar.";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                vObjRsp.Response.Status = false;
                vObjRsp.Response.Message = "Problemas al buscar el registro " + ex.Message;
                return vObjRsp;
            }

            finally
            {
                conn.Close();
            }

            return vObjRsp;
        }

    }
}
