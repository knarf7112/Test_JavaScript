using System;
using System.Linq;
//
using System.Data;

namespace LogTail.Data
{
    public class DbModel : IDbModel
    {
        #region Field
        private IDbConnection _dbConnection = null;
        private string _error = String.Empty;
        private bool _CloseConnection = false;
        #endregion

        #region Property
        public string ConnectionString { get; set; }
        /// <summary>
        /// 記錄Exception的TraceStack
        /// </summary>
        public string Error 
        { 
            get 
            {
                return this._error;
            }
            private set 
            {
                if (OnDbModelException != null)
                {
                    OnDbModelException.Invoke(value);
                }
                this._error = value;
            }
        }

        /// <summary>
        /// 執行完命令後是否關閉連線(Default:false)
        /// </summary>
        public bool CloseConnection
        {
            get
            {
                return this._CloseConnection;
            }
            set
            {
                this._CloseConnection = value;
            }
        }
        #endregion

        #region delegate exception (暫時先不設定拋異常的方式)
        private delegate void DbModelException(string ex);
        private DbModelException OnDbModelException = null;
        private void ThrowDbModelException(string ex)
        {
            throw new Exception(ex);
        }
        #endregion

        #region Constructor
        public DbModel(IDbConnection conn)
        {
            this._dbConnection = conn;
            //this._dbCommand = this._dbConnection.CreateCommand();
        }
        public DbModel(IDbConnection conn, string connectionString)
            : this(conn)
        {
            //this.OnDbModelException = new DbModelException(ThrowDbModelException);
            this._dbConnection.ConnectionString = connectionString;
        }
        #endregion

        #region Public Method
        /// <summary>
        /// 執行Command的ExecuteNonQuery
        /// </summary>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>Db資料列變更數量</returns>
        public int ExecNonQuery(string sqlCmd, params IDataParameter[] paras)
        {
            Open();

            IDbCommand dbCmd = this.GetDbCommand(sqlCmd, this._dbConnection, paras);

            try
            {
                int changeCount = dbCmd.ExecuteNonQuery();
                return changeCount;
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                throw ex;
            }
            finally
            {
                dbCmd.Parameters.Clear();
                dbCmd.Dispose();
                dbCmd = null;
                if (this.CloseConnection)
                {
                    this._dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// 執行Command的ExecuteScalar
        /// </summary>
        /// <typeparam name="T">DB傳回的執行結果型別</typeparam>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>SQL命令運算結果</returns>
        public T ExecScalar<T>(string sqlCmd, params IDataParameter[] paras)
        {
            Open();
            
            IDbCommand dbCmd = this.GetDbCommand(sqlCmd, this._dbConnection, paras);
            object result = null;
            try
            {
                result = dbCmd.ExecuteScalar();
                if (result is DBNull)
                {
                    return default(T);
                }
                else
                {
                    return (T)result;
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                throw ex;
            }
            finally
            {
                dbCmd.Parameters.Clear();
                dbCmd.Dispose();
                dbCmd = null;
                if (this.CloseConnection)
                {
                    this._dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// 執行Command的ExecuteReader
        /// 執行完方法會Dispose Command物件
        /// 但連線要自己手動關閉
        /// </summary>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>DataReader(自己讀取資料列)</returns>
        public IDataReader ExecReader(string sqlCmd, params IDataParameter[] paras)
        {
            Open();

            IDbCommand dbCmd = this.GetDbCommand(sqlCmd, this._dbConnection, paras);

            try
            {
               IDataReader dr = dbCmd.ExecuteReader();
               return dr;
            }
            catch (Exception ex)
            {
                this.Error = ex.StackTrace;
                
                dbCmd.Cancel();
                throw ex;
            }
            finally
            {
                dbCmd.Parameters.Clear();
                dbCmd.Dispose();
                dbCmd = null;
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 開啟Db連線
        /// </summary>
        private void Open()
        {
            if (this._dbConnection == null)
            {
                throw new Exception("DBConnection 為 null");
            }
            if (this._dbConnection.State == ConnectionState.Closed)
            {
                try
                {
                    this._dbConnection.Open();
                }
                catch (Exception ex)
                {
                    this.Error = ex.StackTrace;
                    throw ex;
                }
            }

        }

        /// <summary>
        /// 從DbConnection取得新的DbCommand
        /// 設定DataParameter參數,SQL命令,與CommandType是SQL文字命令
        /// </summary>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="dbConnection">Db連線物件</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>設定完畢的DbCommand</returns>
        private IDbCommand GetDbCommand(string sqlCmd, IDbConnection dbConnection, params IDataParameter[] paras)
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandType = CommandType.Text;
            dbCmd.CommandText = sqlCmd;
            //if (paras != null)
            foreach (IDataParameter para in paras.AsEnumerable())
            {
                dbCmd.Parameters.Add(para);
            }
            return dbCmd;
        }
        #endregion

        public void Dispose()
        {
            if (this._dbConnection != null && this._dbConnection.State != ConnectionState.Closed)
            {
                this._dbConnection.Close();
                this._dbConnection.Dispose();
                this._dbConnection = null;
            }
        }
    }
}
