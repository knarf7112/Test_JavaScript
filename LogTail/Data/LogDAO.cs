//
using LogTail.Domain.Entity;
using System;
using System.Data;
using Npgsql;

namespace LogTail.Data
{
    public class LogDAO : IDAO<LogDO, int>
    {
        public string LogTable { get; set; }

        IDbConnection conn;

        public int GetMaxPk()
        {
            string getPk = @"select MAX(Id) from " + this.LogTable;

            conn = new NpgsqlConnection();
            
            try
            {

            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public System.Collections.Generic.IList<LogDO> ListAfter(int pk)
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IList<LogDO> ListByDate(string dateStr)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteBefore(string dateStr)
        {
            throw new System.NotImplementedException();
        }
    }
}
