//
using LogTail.Domain.Entity;
using System;
using System.Data;
using Npgsql;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.IO;

namespace LogTail.Data
{
    public class LogDAO : IDAO<LogDO, int>
    {
        private string _logTable;
        public string LogTable 
        { 
            get 
            {
                if (string.IsNullOrEmpty(_logTable))
                    throw new Exception("table name is null or empty!");
                return this._logTable;
            }
            set 
            {
                this._logTable = value;
            }
        }
        private DbModel dbModel;

        public LogDAO()
        {
            
            this.dbModel = new DbModel(new NpgsqlConnection());
        }

        public void LoadConfig()
        {
            
            
        }

        public int GetMaxPk()
        {
            string getPk = @"select MAX(Id) from " + this.LogTable;

            //conn = new NpgsqlConnection();
            
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
