using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace LotterySpider.DataBase
{
    public static class DBHelper
    {
        private static SQLiteConnection SqlConn = null;
        public static SQLiteConnection SQLConn
        {
            get
            {                
                return SqlConn;
            }
        }
        public static void LoadDB(string DBName)
        {
            SqlConn = null;
            if (!String.IsNullOrWhiteSpace(DBName))
            {
                string dbPath = "Data Source =" + Environment.CurrentDirectory + "/" + DBName;
                SqlConn = new SQLiteConnection(dbPath);
                SqlConn.Open();
            }
        }
        public static bool CreateTable(string CreateSqlStr)
        {
            if (SqlConn != null&& !String.IsNullOrWhiteSpace(CreateSqlStr))
            {
                SQLiteCommand sqlCmd = new SQLiteCommand(CreateSqlStr,SqlConn);
                sqlCmd.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static SQLiteDataReader Query(string queryStr)
        {
            SQLiteCommand cmd = new SQLiteCommand(queryStr, SQLConn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }
        public static bool Delete(string sqlStr)
        {
            SQLiteCommand cmd = new SQLiteCommand(sqlStr,SQLConn);
            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
