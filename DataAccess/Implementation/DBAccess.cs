using Microsoft.Extensions.Configuration;
using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public class DBAccess : IDBAccess
    {
        IConfiguration _config;
        public DBAccess(IConfiguration config)
        {
            _config = config;
        }
        public ResponseClass Exec_SP(SqlCommand sqlCommand)
        {
            ResponseClass response = new ResponseClass();
            try
            {
                string connectionString = _config.GetConnectionString("SqlConnectionString");
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlCommand.Connection = sqlConnection;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataSet tableData = new DataSet();
                sqlConnection.Open();
                sqlDataAdapter.Fill(tableData);
                sqlConnection.Close();
                response.IsSuccess = true;
                response.ResultData = tableData;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }

}
