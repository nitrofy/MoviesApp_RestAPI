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
    public class Movie : IMovie
    {
        private IDBAccess dBAccess;
        public Movie(IDBAccess _dBAccess)
        {
            dBAccess = _dBAccess;
        }
        public ResponseClass AddUpdateMovie(MovieDetailRequest movieDetail)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                #region Validation
                response.IsSuccess = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                movieDetail.ActivityType = movieDetail.ActivityType.ToUpper();
                if (!new string[] { "D", "U", "C" }.Contains(movieDetail.ActivityType))
                {
                    response.Message = "Invalid flag for operation"; return response;
                }
                if (new string[] { "D", "U" }.Contains(movieDetail.ActivityType) && movieDetail.MovieId == 0)
                {
                    response.Message = "MovieId is required to update or delete."; return response;
                }
                if (new string[] { "C", "U" }.Contains(movieDetail.ActivityType))
                {
                    if (string.IsNullOrEmpty(movieDetail.MovieName))
                    {
                        response.Message = "MovieName is required to Create or Update a record."; return response;
                    }
                }

                #endregion Validation

                DataTable TempArrayTable = new DataTable();
                TempArrayTable.Columns.Add("ArrayItem", typeof(int));
                if (movieDetail.ActorIds != null && movieDetail.ActorIds.Count() > 0)
                {
                    foreach (int actorId in movieDetail.ActorIds)
                    {
                        TempArrayTable.Rows.Add(actorId);
                    }
                }

                string queryOne = "sp_AddUpdateMovie";
                SqlCommand sqlCommandOne = new SqlCommand(queryOne);
                sqlCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommandOne.Parameters.AddWithValue("MovieId", movieDetail.MovieId);
                sqlCommandOne.Parameters.AddWithValue("MovieName", movieDetail.MovieName??"");
                sqlCommandOne.Parameters.AddWithValue("Plot", movieDetail.Plot ?? "");
                sqlCommandOne.Parameters.AddWithValue("DateOfRelease", movieDetail.DateOfRelease.Date == DateTime.MinValue ? "1900-01-01" : movieDetail.DateOfRelease.Date);
                sqlCommandOne.Parameters.AddWithValue("ProducerId", movieDetail.ProducerId);
                sqlCommandOne.Parameters.AddWithValue("Flag", movieDetail.ActivityType ?? "");
                SqlParameter actorIdParameter = sqlCommandOne.Parameters.AddWithValue("ActorIds", TempArrayTable);
                actorIdParameter.SqlDbType = SqlDbType.Structured;
                actorIdParameter.TypeName = "dbo.TempArrayTable";
                

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
         public ResponseClass MoviePoster(MoviePosterRequest moviePosterRequest)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                #region Validation
                response.IsSuccess = false;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                moviePosterRequest.ActivityType = moviePosterRequest.ActivityType.ToUpper();
                if (!new string[] { "D", "U", "C","R" }.Contains(moviePosterRequest.ActivityType))
                {
                    response.Message = "Invalid flag for operation"; return response;
                }
                if (new string[] { "D", "U","R" }.Contains(moviePosterRequest.ActivityType) && moviePosterRequest.MovieId == 0)
                {
                    response.Message = "MovieId is required!"; return response;
                }
                if (new string[] { "C", "U" }.Contains(moviePosterRequest.ActivityType))
                {
                    if (string.IsNullOrEmpty(moviePosterRequest.MoviePoster))
                    {
                        response.Message = "MoviePoster (Base64 String) is required to Add or Update."; return response;
                    }
                }

                #endregion Validation

                string queryOne = "sp_AddUpdatePoster";
                SqlCommand sqlCommandOne = new SqlCommand(queryOne);
                sqlCommandOne.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommandOne.Parameters.AddWithValue("MovieId", moviePosterRequest.MovieId);
                sqlCommandOne.Parameters.AddWithValue("Poster", moviePosterRequest.MoviePoster??"");
                sqlCommandOne.Parameters.AddWithValue("Flag", moviePosterRequest.ActivityType ?? "");

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
                        else if (dataSet.Tables[0].Rows[0]["Status"].ToString() == "2")
                        {
                            List<MoviePosterResponse> moviePoster = ExtensionMethods.GetEntityList<MoviePosterResponse>(dataSet.Tables[0]);
                            response.ResultData = moviePoster;
                            response.IsSuccess = true;
                            response.Message = "Success";
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
        public ResponseClass GetMoviesList(bool detail = false)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                string queryOne = detail ? "sp_GetMoviesListDetail" : "sp_GetMoviesListShort";
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
                            List<MoviesDetailResponse> moviesListDetail = ExtensionMethods.GetEntityList<MoviesDetailResponse>(dataSet.Tables[0]);
                            if (moviesListDetail.Count() > 0)
                            {
                                List<MovieActorRelation> actorsList = ExtensionMethods.GetEntityList<MovieActorRelation>(dataSet.Tables[1]);
                                moviesListDetail.ForEach(x =>
                                {
                                    List<MovieActorRelation> templist = actorsList.Where(c => c.MovieId == x.MovieId).ToList();
                                    x.ActorsList = new List<ActorShort>();
                                    foreach (var item in templist)
                                    {
                                        x.ActorsList.Add(new ActorShort { ActorId = item.ActorId, ActorName = item.ActorName });
                                    }
                                });
                            }
                            response.ResultData = moviesListDetail;
                        }
                        else
                        {
                            List<MovieShort> moviesList = ExtensionMethods.GetEntityList<MovieShort>(dataSet.Tables[0]);
                            response.ResultData = moviesList;
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
