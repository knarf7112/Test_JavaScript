//
using LogTail.Domain.Entity;
using System;
using System.Data;
using Npgsql;
using System.Collections.Generic;
using LogTail.Config;
using LogTail.Domain.Mapper;

namespace LogTail.Data
{
    public class LogDAO : IDAO<LogDO, int>
    {
        private string _logTable = string.Empty;
        public string CurrentSelectLogTable 
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

        public string CurrentSelectConnectionString { get; set; }

        public string Error { get; set; }

        //public IDictionary<string, string> ConnectionStringDic { get; set; }
        //public IList<string> LogTableList { get; set; }
        private IDbModel dbModel { get; set; }
        private IRowMapper<LogDO> mapper { get; set; }
        public LogDAO()
        {
            LoadConfig(@"\Config\DBConfig.xml");
            this.dbModel = new DbModel(new NpgsqlConnection(this.CurrentSelectConnectionString));
            this.mapper = new RowMapper<LogDO>();
        }

        /// <summary>
        /// 暫時直接取檔案某個選擇的table(未完整)
        /// 應該要提供table列表給前端選擇
        /// </summary>
        /// <param name="path"></param>
        private void LoadConfig(string path)
        {
            XmlFileReader xml = new XmlFileReader(path);
            this.CurrentSelectConnectionString = xml.GetNodeValue("ConnectionString", "select", "true");
            this.CurrentSelectLogTable = xml.GetNodeAttributeValue("table", "name", "select", "true");

        }

        public int GetMaxPk()
        {
            string getPk = @"select MAX(Id) from " + this.CurrentSelectLogTable;


            try
            {
                int result = this.dbModel.ExecScalar<int>(getPk);
                return result;
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                //return 0;
                throw ex;
            }
        }

        public IList<LogDO> ListAfter(int pk)
        {
            string cmdText = @"select 
                                    Id, Date, Thread, Level, Logger, Message, Exception, HostId 
                               from " + this.CurrentSelectLogTable + @" 
                               where Id > :Id 
                               order by Id";
            IDataReader dr = null;
            try
            {
                IDataParameter para = new NpgsqlParameter();
                para.Direction = ParameterDirection.Input;
                para.DbType = DbType.Int32;
                para.ParameterName = "Id";
                para.Value = pk;

                dr = this.dbModel.ExecReader(cmdText, para);
                return this.mapper.AllRowMapping(dr);
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                //return null;
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                    dr = null;
                }
            }
        }

        public IList<LogDO> ListByDate(string dateStr)
        {
            string cmdText =
                @"select 
                    Id, Date, Thread, Level, Logger, Message, Exception, HostId
                  from " + this.CurrentSelectLogTable + @"
                  where to_char( Date, 'YYYYMMDD' ) = :Date 
                  order by Id
                 ";
            IDataReader dr = null;
            try
            {
                IDataParameter para = new NpgsqlParameter() 
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "Date",
                    Value = dateStr,
                };
                
                
                dr = this.dbModel.ExecReader(cmdText, para);
                return this.mapper.AllRowMapping(dr);
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                //return null;
                throw ex;
            }
            finally
            {
                if (dr != null)
                {
                    dr.Close();
                    dr.Dispose();
                    dr = null;
                }
            }

        }

        public void DeleteBefore(string dateStr)
        {
            string cmdText =
                @"delete 
                  from " + this.CurrentSelectLogTable + @"
                  where to_char( Date, 'YYYYMMDD' ) < :Date 
                 "
            ;
            try
            {
                IDataParameter para = new NpgsqlParameter()
                {
                    Direction = ParameterDirection.Input,
                    DbType = DbType.String,
                    ParameterName = "Date",
                    Value = dateStr,
                };

                this.dbModel.ExecNonQuery(cmdText, para);
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                throw ex;
            }
        }
    }
}
