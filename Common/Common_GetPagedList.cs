using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SqlClient;
namespace Common
{
    public  class Common_GetPagedList<T>
    {

        string SqlConnnectionString;

        public Common_GetPagedList()
        {

        }

        public Common_GetPagedList(string sqlConnnectionString)
        {
            SqlConnnectionString = sqlConnnectionString;
        }

        public IEnumerable<T> ExecuteStoredProcedureWithParms(string TableName, string ColumnNames, string OrderClause, string WhereClause, int  PageSize, int PageIndex, out int TotalRecord)
        {
            TotalRecord = 0;
            DynamicParameters p = new DynamicParameters();
            p.Add("@TableName", TableName, DbType.String);
            p.Add("@ColumnNames", ColumnNames, DbType.String);
            p.Add("@OrderClause", OrderClause, DbType.String);
            p.Add("@WhereClause", WhereClause, DbType.String);
            p.Add("@PageSize", PageSize, DbType.Int32);
            p.Add("@PageIndex", PageIndex, DbType.Int32);
            p.Add("@TotalRecord", TotalRecord, DbType.Int32,ParameterDirection.InputOutput);
            using (var conn = new System.Data.SqlClient.SqlConnection(SqlConnnectionString))
            {
             
                IEnumerable<T> list= conn.Query<T>("Common_GetPagedList", p, null, true, null, CommandType.StoredProcedure);

               TotalRecord = p.Get<int>("@TotalRecord");

                return list;
               
            }
        }
    }
}
