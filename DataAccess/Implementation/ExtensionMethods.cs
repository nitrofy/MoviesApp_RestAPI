using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public static class ExtensionMethods
    {
        public static List<T> GetEntityList<T>(DataTable dataTable)
        {
            List<T> dataList = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T objectT = GetItem<T>(row);
                dataList.Add(objectT);
            }
            return dataList;
        }
        private static T GetItem<T>(DataRow row)
        {
            Type genericType = typeof(T);
            T objectT = Activator.CreateInstance<T>();
            foreach (DataColumn column in row.Table.Columns)
            {
                foreach (PropertyInfo prop in genericType.GetProperties())
                {
                    if (prop.Name == column.ColumnName)
                        prop.SetValue(objectT, row[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return objectT;
        }
    }

}
