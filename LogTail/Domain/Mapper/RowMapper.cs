using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using LogTail.Domain.Entity;
using System.Data;
using System.Reflection;

namespace LogTail.Domain.Mapper
{
    /// <summary>
    /// 用DataReader讀取Row資料映射到物件的屬性上
    /// IDataReader ref:https://msdn.microsoft.com/zh-tw/library/system.data.idatareader(v=vs.110).aspx
    /// </summary>
    /// <typeparam name="T">要映射的資料物件型別</typeparam>
    public class RowMapper<T> : IRowMapper<T> where T : LogDO, new()
    {
        private bool HasData = false;
        
        /// <summary>
        /// 用Reflection方式從IDataReader把資料表欄位值映射到資料物件的屬性上
        /// </summary>
        /// <param name="dataReader">IDataReader的端口</param>
        /// <returns>資料物件</returns>
        public T RowMapping(IDataReader dataReader)
        {
            HasData = dataReader.Read() ? true : false;//如果dataReader前進到下一個資料錄後,有東西的話
            if (this.HasData)
            {
                T row = new T();
                //BindingFlags bf = BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo[] pi = row.GetType().GetProperties();//取得物件的所有屬性
                if (pi.Length != dataReader.FieldCount)
                {
                    throw new Exception("物件屬性數量(" + pi.Length + ")與資料庫Column數量(" + dataReader.FieldCount + ")不同,無法正確映射");
                }
                for(int i = 0; i < pi.Length; i++)
                {
                    pi[i].SetValue(row, dataReader.GetValue(i), null);//將Row上的欄位值依序映射到此物件的屬性上
                }
                return row;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 取得所有資料列的資料物件
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>資料物件的集合列表</returns>
        public IList<T> AllRowMapping(IDataReader dataReader)
        {
            IList<T> list = new List<T>();
            do
            {
                T poco = RowMapping(dataReader);
                if (poco != null)
                {
                    list.Add(poco);
                }
            }
            while (this.HasData);
            return list;
        }
    }
}
