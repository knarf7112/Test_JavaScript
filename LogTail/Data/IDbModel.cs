using System;
using System.Data;

namespace LogTail.Data
{
    /// <summary>
    /// DB模組(注入Connection)
    /// 注入的類別需要有實作IDbConnection,IDbCommand,IDbDataParameter,IDataReader才能用
    /// </summary>
    public interface IDbModel: IDisposable
    {
        /// <summary>
        /// 執行Command的ExecuteNonQuery
        /// </summary>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>Db資料列變更數量</returns>
        int ExecNonQuery(string sqlCmd, params IDbDataParameter[] paras);

        /// <summary>
        /// 執行Command的ExecuteScalar
        /// </summary>
        /// <typeparam name="T">DB傳回的執行結果型別</typeparam>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>SQL命令運算結果</returns>
        T ExecScalar<T>(string sqlCmd, params IDbDataParameter[] paras);

        /// <summary>
        /// 執行Command的ExecuteReader
        /// Command已Dispose
        /// </summary>
        /// <param name="sqlCmd">SQL命令字串</param>
        /// <param name="paras">DbCommand要加入的Parametter</param>
        /// <returns>DataReader(自己讀取資料列)</returns>
        IDataReader ExecReader(string sqlCmd, params IDbDataParameter[] paras);
    }
}
