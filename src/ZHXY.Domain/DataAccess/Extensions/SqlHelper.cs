using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ZHXY.Domain
{
    public class SqlHelper
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public static string GetSqlConnection()
        {
            return "Data Source = 210.35.32.29; Initial Catalog = hw_WisCampus_test_Dorm; User ID = sa; Password = hw123!@#";
        }

        /// <summary>
        /// 封装一个执行的sql 返回受影响的行数
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteNonQuery(string sqlText)
        {
            using (var conn = new SqlConnection(GetSqlConnection()))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sqlText;
                    //cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>受影响的行数</returns>
        public static int ExecuteProcedure(string procedure, Dictionary<string, Object> para)
        {
            using (var conn = new SqlConnection(GetSqlConnection()))
            {
                using (var cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //把具体的值传给输入参数
                    foreach (var kv in para)
                    {
                        cmd.Parameters.Add(new SqlParameter(kv.Key, kv.Value));
                    }
                    //执行存储过程
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行sql，返回查询结果中的第一行第一列的值
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>查询结果中的第一行第一列的值</returns>
        public static object ExecuteScalar(string sqlText, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(GetSqlConnection()))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = sqlText;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// 执行sql 返回一个DataTable
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回一个DataTable</returns>
        public static DataTable ExecuteDataTable(string sqlText, params SqlParameter[] parameters)
        {
            using (var adapter = new SqlDataAdapter(sqlText, GetSqlConnection()))
            {
                var dt = new DataTable();
                adapter.SelectCommand.Parameters.AddRange(parameters);
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// 执行sql脚本
        /// </summary>
        /// <param name="sqlText">执行的sql脚本</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>返回一个SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string sqlText, params SqlParameter[] parameters)
        {
            //SqlDataReader要求，它读取数据的时候有，它独占它的SqlConnection对象，而且SqlConnection必须是Open状态
            var conn = new SqlConnection(GetSqlConnection());//不要释放连接，因为后面还需要连接打开状态
            var cmd = conn.CreateCommand();
            conn.Open();
            cmd.CommandText = sqlText;
            cmd.Parameters.AddRange(parameters);
            //CommandBehavior.CloseConnection当SqlDataReader释放的时候，顺便把SqlConnection对象也释放掉
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 判断数据库表是否存在，没有就直接创建。
        /// </summary>
        /// <param name="tablename">bhtsoft表</param>
        /// <returns></returns>
        public static void CheckExistsTable(string tablename, string createTabelSql)
        {
            using (var conn = new SqlConnection(GetSqlConnection()))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var tableNameStr = "select count(1) from sysobjects where name = '" + tablename + "'";
                var cmd = new SqlCommand(tableNameStr, conn);
                var result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result == 0)
                {
                    cmd = new SqlCommand(createTabelSql, conn);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //按月分表取得表名
        //ps:prefix+201801
        public static List<string> GetTableNameByMonth(string table_prefix, DateTime dtStart, DateTime dtEnd)
        {
            if (dtStart > dtEnd)
            {
                throw new Exception("参数错误");
            }
            else
            {
                var res = new List<string>();
                var Year = dtEnd.Year - dtStart.Year;
                var Month = ((dtEnd.Year - dtStart.Year) * 12) + (dtEnd.Month - dtStart.Month);
                for (var i = 0; i <= Month; i++)
                {
                    var dt = dtStart.AddMonths(i);
                    res.Add(table_prefix + dt.ToString("yyyyMM"));
                }
                return res;
            }
        }
    }
}