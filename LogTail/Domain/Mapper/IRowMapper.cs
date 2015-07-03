using System.Collections.Generic;
//
using System.Data;

namespace LogTail.Domain.Mapper
{
    public interface IRowMapper<T>
    {

        T RowMapping(IDataReader dataReader);

        /// <summary>
        /// 取得所有資料列的資料物件
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>資料物件的集合列表</returns>
        IList<T> AllRowMapping(IDataReader dataReader);
    }
}
