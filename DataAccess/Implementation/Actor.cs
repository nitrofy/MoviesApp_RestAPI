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
    public class Actor : IActor
    {
        private IDBAccess dBAccess;
        public Actor(IDBAccess _dBAccess)
        {
            dBAccess = _dBAccess;
        }
        public ResponseClass AddUpdateActor(ActorDetailRequest actorDetail)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                #region Validation
                response.IsSuccess = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                actorDetail.ActivityType = actorDetail.ActivityType.ToUpper();
                if (!new string[] { "D", "U", "C" }.Contains(actorDetail.ActivityType))
                {
                    response.Message = "Invalid flag for operation"; return response;
                }
                if (new string[] { "D", "U" }.Contains(actorDetail.ActivityType) && actorDetail.ActorId == 0)
                {
                    response.Message = "ActorId is required to update or delete."; return response;
                }
                if (new string[] { "U", "C" }.Contains(actorDetail.ActivityType))
                {
                    if (string.IsNullOrEmpty(actorDetail.ActorName))
                    {
                        response.Message = "ActorName is required to Create or Update a record."; return response;
                    }
                    if (!string.IsNullOrEmpty(actorDetail.Gender) && !new string[] { "FEMALE", "MALE", "OTHER", "OTHERS" }.Contains(actorDetail.Gender.ToUpper()))
                    {
                        response.Message = "Invalid Gender."; return response;
                    }
                }
                
                #endregion Validation
                string queryOne = "sp_AddUpdateActor";
                SqlCommand sqlCommandOne = new SqlCommand(queryOne);
                sqlCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommandOne.Parameters.AddWithValue("ActorId", actorDetail.ActorId);
                sqlCommandOne.Parameters.AddWithValue("ActorName", actorDetail.ActorName ?? "");
                sqlCommandOne.Parameters.AddWithValue("Bio", actorDetail.Bio ?? "");
                sqlCommandOne.Parameters.AddWithValue("DOB", actorDetail.DOB.Date == DateTime.MinValue ? "1900-01-01": actorDetail.DOB.Date) ;
                sqlCommandOne.Parameters.AddWithValue("Gender", actorDetail.Gender ?? "");
                sqlCommandOne.Parameters.AddWithValue("Flag", actorDetail.ActivityType ?? "");
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
        public ResponseClass GetActorsList(bool detail = false)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                string queryOne = detail ? "sp_GetActorsListDetail" : "sp_GetActorsListShort";
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
                            List<ActorDetail> actorsListDetail = ExtensionMethods.GetEntityList<ActorDetail>(dataSet.Tables[0]);
                            response.ResultData = actorsListDetail;
                        }
                        else
                        {
                            List<ActorShort> actorsList = ExtensionMethods.GetEntityList<ActorShort>(dataSet.Tables[0]);
                            response.ResultData = actorsList;
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
