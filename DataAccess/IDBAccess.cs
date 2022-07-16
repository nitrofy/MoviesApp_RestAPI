using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public interface IDBAccess
    {
        public ResponseClass Exec_SP(SqlCommand sqlCommand);
    }
}
