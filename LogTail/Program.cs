using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Data;
using Npgsql;
using LogTail.Domain.Mapper;
using LogTail.Domain.Entity;

namespace LogTail
{
    class Program
    {
        /// <summary>
        /// test PostgresSQL
        /// ref:http://www.mono-project.com/docs/database-access/providers/postgresql/
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            IRowMapper<LogDO> qq = new RowMapper<LogDO>();//
            
            string connectionString = "Server=10.27.68.151;Port=5432;User Id=test;Password=test123;Database=logdb;";
            string sqlCmd = @"select * from socketlog order by id limit 10";
            IDbConnection dbCon = new NpgsqlConnection(connectionString);
            dbCon.Open();
            //************************************************************************
            //IDbCommand dbCmd = new NpgsqlCommand(sqlCmd, (NpgsqlConnection)dbCon);
            IDbCommand dbCmd = dbCon.CreateCommand();
            //dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = sqlCmd;
            //************************************************************************
            IDataReader dr = dbCmd.ExecuteReader();
            RowMapper<LogDO> list = new RowMapper<LogDO>();
            IEnumerable<LogDO> enumerable = list.AllRowMapping(dr);
            foreach (var item in enumerable)
            {
                Console.WriteLine("*************************************************");
                Console.WriteLine(item.ToString());
                Console.WriteLine("*************************************************");
            }
            //while (dr.Read())
            //{
            //    var column1 = dr.GetValue(0);
            //    var column2 = dr.GetValue(1);
            //    var column3 = dr.GetValue(2);
            //    var column4 = dr.GetValue(3);
            //    var column5 = dr.GetValue(4);
            //    var column6 = dr.GetValue(5);
            //    var test = dr.GetOrdinal("id");
            //    Console.WriteLine("*************************************************");
            //    Console.WriteLine("第1欄:" + column1);
            //    Console.WriteLine("第2欄:" + column2);
            //    Console.WriteLine("第3欄:" + column3);
            //    Console.WriteLine("第4欄:" + column4);
            //    Console.WriteLine("第5欄:" + column5);
            //    Console.WriteLine("第6欄:" + column6);
            //    Console.WriteLine("*************************************************");
            //}
            dr.Close();
            dr = null;
            dbCmd.Dispose();
            dbCmd = null;
            dbCon.Close();
            dbCon = null;
            Console.ReadKey();
        }
    }
}
