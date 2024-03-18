using InvoiceData.Interfaces;
using InvoiceModel;
using InvoiceModel.ModelRsp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace InvoiceData
{
    public class InvoicesData : IInvoiceData
    {
        private readonly ILogger<InvoicesData> _logger;
        private readonly IConfiguration _config;

        public InvoicesData(ILogger<InvoicesData> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        private const string InvoiceHeaderAdd = "InvoiceHeader_Add";
        private const string InvoiceBodyAdd = "InvoiceBody_Add";
        private const string InvoiceValidate = "InvoiceValidate";
        private const string InvoiceRpt = "invoices_report";

        public async Task<Response> InvHeaderAddAsync(InvoiceHeader vInvoiceHeader)
        {
            Response vObjRsp = new();

            SqlConnection conn = new(_config["ConnectionStrings:SqlServer"]);
            try
            {
                SqlCommand StoreProc_enc = new(InvoiceHeaderAdd, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                StoreProc_enc.Parameters.Add("@DateInv", SqlDbType.Date).Value = vInvoiceHeader.DateInv;
                StoreProc_enc.Parameters.Add("@DocumentClient", SqlDbType.Int).Value = vInvoiceHeader.DocumentClient;
                StoreProc_enc.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = vInvoiceHeader.Name;
                StoreProc_enc.Parameters.Add("@LastName", SqlDbType.VarChar, 50).Value = vInvoiceHeader.LastName;
                StoreProc_enc.Parameters.Add("@AddressClient", SqlDbType.VarChar, 100).Value = vInvoiceHeader.AddressClient;
                StoreProc_enc.Parameters.Add("@PhoneClient", SqlDbType.VarChar, 10).Value = vInvoiceHeader.PhoneClient;
                //begintran
                conn.Open();
                using SqlDataReader reader = await StoreProc_enc.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["HasErrors"]) == 0)
                    {
                        int IdInvoice = reader["IdInvoice"] != DBNull.Value ? Convert.ToInt32(reader["IdInvoice"].ToString()) : 0;

                        if (IdInvoice > 0 && vInvoiceHeader.InvoiceBodys.Count > 0)
                        {
                            foreach (var item in vInvoiceHeader.InvoiceBodys)
                            {
                                item.IdInvoice = IdInvoice;
                                await InvBodyAddAsync(item);
                            }
                        }

                        vObjRsp.Status = true;
                        vObjRsp.Message = "Se creo la factura numero " + IdInvoice;
                    }
                    else
                    {
                        vObjRsp.Status = false;
                        vObjRsp.Message = "El registro no se logro guardar";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                vObjRsp.Status = false;
                vObjRsp.Message = "Problemas al crear el registro " + ex.Message;
                return vObjRsp;
            }

            finally
            {
                conn.Close();
            }

            return vObjRsp;
        }

        public async Task<Response> InvBodyAddAsync(InvoiceBody vInvoiceBody)
        {
            Response vObjRsp = new();

            SqlConnection conn = new SqlConnection(_config["ConnectionStrings:SqlServer"]);
            try
            {
                SqlCommand StoreProc_enc = new SqlCommand(InvoiceBodyAdd, conn);
                StoreProc_enc.CommandType = CommandType.StoredProcedure;

                StoreProc_enc.Parameters.Add("@ItemId", SqlDbType.Int).Value = vInvoiceBody.ItemId;
                StoreProc_enc.Parameters.Add("@ItemQuantity", SqlDbType.Int).Value = vInvoiceBody.ItemQuantity;
                StoreProc_enc.Parameters.Add("@Price", SqlDbType.Decimal).Value = vInvoiceBody.Price;
                StoreProc_enc.Parameters.Add("@GrossValue", SqlDbType.Decimal).Value = vInvoiceBody.GrossValue;
                StoreProc_enc.Parameters.Add("@ValueDiscount", SqlDbType.Decimal).Value = vInvoiceBody.ValueDiscount;
                StoreProc_enc.Parameters.Add("@IvaAmout", SqlDbType.Decimal).Value = vInvoiceBody.IvaAmout;
                StoreProc_enc.Parameters.Add("@TotalValue", SqlDbType.Decimal).Value = vInvoiceBody.TotalValue;
                StoreProc_enc.Parameters.Add("@IdInvoice", SqlDbType.Int).Value = vInvoiceBody.IdInvoice;

                conn.Open();
                using (SqlDataReader reader = await StoreProc_enc.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["HasErrors"]) == 0)
                        {
                            int IdInvoice = 0;

                            vObjRsp.Status = true;
                            vObjRsp.Message = "Se creo la factura exitosamente";
                        }
                        else
                        {
                            vObjRsp.Status = false;
                            vObjRsp.Message = "El registro no se logro guardar";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                vObjRsp.Status = false;
                vObjRsp.Message = "Problemas al crear el registro " + ex.Message;
                return vObjRsp;
            }

            finally
            {
                conn.Close();
            }

            return vObjRsp;
        }

        #region Nueva funcionalidad
        public async Task<Response> InvValidateAsync(Int64 vDocumentClient)
        {
            Response vObjRsp = new();

            SqlConnection conn = new SqlConnection(_config["ConnectionStrings:SqlServer"]);
            try
            {
                SqlCommand StoreProc_enc = new SqlCommand(InvoiceValidate, conn);
                StoreProc_enc.CommandType = CommandType.StoredProcedure;

                StoreProc_enc.Parameters.Add("@DocumentClient", SqlDbType.Int).Value = vDocumentClient;


                conn.Open();
                using (SqlDataReader reader = await StoreProc_enc.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["HasErrors"]) == 1)
                        {

                            vObjRsp.Status = true;
                            vObjRsp.Message = reader["MsgError"] != DBNull.Value ? reader["MsgError"].ToString() : string.Empty; ;
                        }
                        else
                        {
                            vObjRsp.Status = false;
                            vObjRsp.Message = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                vObjRsp.Status = false;
                vObjRsp.Message = "Problemas al validar registros. " + ex.Message;
                return vObjRsp;
            }

            finally
            {
                conn.Close();
            }

            return vObjRsp;
        }        

        public async Task<RspRptInvoices> RspRptInvoices()
        {
            RspRptInvoices vObjRsp = new();

            SqlConnection conn = new(_config["ConnectionStrings:SqlServer"]);
            try
            {
                SqlCommand StoreProc_enc = new(InvoiceRpt, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                //StoreProc_enc.Parameters.Add("@DateInv", SqlDbType.Date).Value = vInvoiceHeader.DateInv;

                conn.Open();
                using SqlDataReader reader = await StoreProc_enc.ExecuteReaderAsync();
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["HasErrors"]) == 0)
                    {
                        RptInvoices vObjRptInv = new();
                        vObjRptInv.IdInvoice = reader["IdInvoice"] != DBNull.Value ? Convert.ToInt32(reader["IdInvoice"].ToString()) : 0;
                        vObjRptInv.DateInv = reader["DateInv"] != DBNull.Value ? reader["DateInv"].ToString() : string.Empty;
                        vObjRptInv.DocumentClient = reader["DocumentClient"] != DBNull.Value ? Convert.ToInt64(reader["DocumentClient"].ToString()) : 0;
                        vObjRptInv.NameClient = reader["NameClient"] != DBNull.Value ? reader["NameClient"].ToString() : string.Empty;
                        vObjRptInv.AddressClient = reader["AddressClient"] != DBNull.Value ? reader["AddressClient"].ToString() : string.Empty;
                        vObjRptInv.PhoneClient = reader["PhoneClient"] != DBNull.Value ? reader["PhoneClient"].ToString() : string.Empty;
                        vObjRptInv.DescItem = reader["DescItem"] != DBNull.Value ? reader["DescItem"].ToString() : string.Empty;
                        vObjRptInv.ItemQuantity = reader["ItemQuantity"] != DBNull.Value ? Convert.ToInt32(reader["ItemQuantity"].ToString()) : 0;
                        vObjRptInv.Price = reader["Price"] != DBNull.Value ? Convert.ToDecimal(reader["Price"].ToString()) : 0;
                        vObjRptInv.GrossValue = reader["GrossValue"] != DBNull.Value ? Convert.ToDecimal(reader["GrossValue"].ToString()) : 0;
                        vObjRptInv.ValueDiscount = reader["ValueDiscount"] != DBNull.Value ? Convert.ToDecimal(reader["IdInvoice"].ToString()) : 0;
                        vObjRptInv.IvaAmout = reader["IvaAmout"] != DBNull.Value ? Convert.ToDecimal(reader["IvaAmout"].ToString()) : 0;
                        vObjRptInv.TotalValue = reader["TotalValue"] != DBNull.Value ? Convert.ToDecimal(reader["TotalValue"].ToString()) : 0;

                        vObjRsp.LstInvoces?.Add(vObjRptInv);
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

        #endregion
    }
}
