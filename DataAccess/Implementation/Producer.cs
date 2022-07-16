using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public class Producer : IProducer
    {
        private IDBAccess dBAccess;
        public Producer(IDBAccess _dBAccess)
        {
            dBAccess = _dBAccess;
        }
        public ResponseClass AddUpdateProducer(ProducerDetailRequest producerDetail)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                #region Validation
                response.IsSuccess = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                producerDetail.ActivityType = producerDetail.ActivityType.ToUpper();
                if (!new string[] { "D", "U", "C" }.Contains(producerDetail.ActivityType))
                {
                    response.Message = "Invalid flag for operation"; return response;
                }
                if (new string[] { "D", "U" }.Contains(producerDetail.ActivityType) && producerDetail.ProducerId == 0)
                {
                    response.Message = "ProducerId is required to update or delete."; return response;
                }
                if (new string[] { "U", "C" }.Contains(producerDetail.ActivityType))
                {
                    if (string.IsNullOrEmpty(producerDetail.ProducerName))
                    {
                        response.Message = "ProducerName is required to Create or Update a record."; return response;
                    }
                    if (!string.IsNullOrEmpty(producerDetail.Gender) && !new string[] { "FEMALE", "MALE", "OTHER", "OTHERS" }.Contains(producerDetail.Gender.ToUpper()))
                    {
                        response.Message = "Ivalid Gender."; return response;
                    }
                }
               
                #endregion Validation
                string queryOne = "sp_AddUpdateProducer";
                SqlCommand sqlCommandOne = new SqlCommand(queryOne);
                sqlCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommandOne.Parameters.AddWithValue("ProducerId", producerDetail.ProducerId);
                sqlCommandOne.Parameters.AddWithValue("ProducerName", producerDetail.ProducerName ?? "");
                sqlCommandOne.Parameters.AddWithValue("Bio", producerDetail.Bio ?? "");
                sqlCommandOne.Parameters.AddWithValue("Company", producerDetail.Company ?? "");
                sqlCommandOne.Parameters.AddWithValue("DOB", producerDetail.DOB.Date == DateTime.MinValue ? "1900-01-01" : producerDetail.DOB.Date);
                sqlCommandOne.Parameters.AddWithValue("Gender", producerDetail.Gender ?? "");
                sqlCommandOne.Parameters.AddWithValue("Flag", producerDetail.ActivityType ?? "");
                ResponseClass dbResponse = dBAccess.Exec_SP(sqlCommandOne);
                if (dbResponse.IsSuccess)
                {
                    DataSet dataSet = (DataSet)dbResponse.ResultData;
                    if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        if (dataSet.Tables[0].Rows[0]["Status"].ToString() == "0")
                        {
                            response.IsSuccess = true;
                            response.Message = dataSet.Tables[0].Rows[0]["Message"].ToString();
                            response.StatusCode = (int)HttpStatusCode.Created;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = dataSet.Tables[0].Rows[0]["Message"].ToString();
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Server Error / Blank response from Database";
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = dbResponse.Message;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            return response;
        }
        public ResponseClass GetProducersList(bool detail = false)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                string queryOne = detail ? "sp_GetProducersListDetail" : "sp_GetProducersListShort";
                SqlCommand sqlCommandOne = new SqlCommand(queryOne);
                sqlCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                ResponseClass dbResponse = dBAccess.Exec_SP(sqlCommandOne);
                if (dbResponse.IsSuccess)
                {
                    DataSet dataSet = (DataSet)dbResponse.ResultData;
                    if (dataSet != null && dataSet.Tables.Count > 0)
                    {
                        if (detail)
                        {
                            List<ProducerDetail> producersListDetail = ExtensionMethods.GetEntityList<ProducerDetail>(dataSet.Tables[0]);
                            response.ResultData = producersListDetail;
                        }
                        else
                        {
                            List<ProducerShort> producersList = ExtensionMethods.GetEntityList<ProducerShort>(dataSet.Tables[0]);
                            response.ResultData = producersList;
                        }
                        response.IsSuccess = true;
                        response.Message = "Success";
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Server Error / Blank response from Database";
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    }
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = dbResponse.Message;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
