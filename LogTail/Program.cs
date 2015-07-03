#define TestDumper
using System;
using System.Collections.Generic;
using System.Linq;

//Postgres test need Assembly
using System.Data;
using Npgsql;
using LogTail.Domain.Mapper;
using LogTail.Domain.Entity;

//load xml file need
using LogTail.Config;
using System.Xml.Linq;
using System.IO;
using System.Collections;

//
using LogTail.Business;

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

#if TestDumper
            LogDumper log = new LogDumper();
            IList<LogDO> list = log.Operate();
            foreach (var item in list)
            {
                Console.WriteLine(item.ToString());
            }
            //JSonConvert
#endif

#if TestXmlFileReader
#warning 這邊會在Compiler的輸出警告的地方出現
            //
            XmlFileReader xx = new XmlFileReader(@"Config\DBConfig.xml");
            string connectionStringtest = xx.GetNodeValue("ConnectionString", "select", "true");
            string tableName = xx.GetNodeAttributeValue("table", "name", "select", "true");
            //var dic = new Dictionary<string,object>();
            //xx.Parse(dic, xx.xEle);
            //--------------------------------------------------------------------------
            XDocument xDoc = XDocument.Load(new StreamReader(@"Config\DBConfig.xml"));
            //XElement xe = xDoc.Root;
//#error Compiler會跳一下錯誤
            IEnumerable<XElement> query = xDoc.Descendants("ConnectionString");
            string connectionString2 = query.Where((XElement x) => 
            {
                return (x.Attribute("select").Value.ToLower() == "true");
            }).First().Value.Trim();
            var qq2 = query;
            //************************************************************************
            //Node parent = new Node();
            //var xDoc = XDocument.Load(new StreamReader("Config/DBConfig.xml"));
            //XmlFileReader.Parse(parent, xDoc.Elements().First());
            //var qq2 = parent;
#endif

#if TestPostgresSQL
            //************************************************************************
            #region 以下為NpgSql 的資料庫連線測試與RowMapper測試
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
            dbCmd.Dispose();//Command關閉不影響DataReader讀取資料,但連線關閉就不能用了,再Open連線也沒用
            //----------------------------------------
            //關閉連線再重開的測試
            //dbCon.Close();
            //dbCon.Open();
            //dbCmd = dbCon.CreateCommand();
            //dbCmd.CommandText = sqlCmd;
            //dr = dbCmd.ExecuteReader();
            //----------------------------------------
            //dbCmd = null;
            RowMapper<LogDO> list = new RowMapper<LogDO>();
            IEnumerable<LogDO> enumerable = list.AllRowMapping(dr);
            foreach (var item in enumerable)
            {
                Console.WriteLine("*************************************************");
                Console.WriteLine(item.ToString());
                //Console.WriteLine(item.Id.GetType());
                //Console.WriteLine(item.Date.GetType());
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
            #endregion
#endif
            //****************************************************************************
            Console.ReadKey();
        }
    }
}
