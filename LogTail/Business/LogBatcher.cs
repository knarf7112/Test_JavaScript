using System;
using System.Collections.Generic;
//
using LogTail.Data;
using LogTail.Common;
using LogTail.Domain.Entity;

namespace LogTail.Business
{
    public class LogBatcher : ILogBatcher
    {
        public string DateStr { get; set; }

        public LogDAO LogDAO { get; set; }

        public IDateUtility DateUtility { get; set; }

        public LogBatcher()
        {
            this.DateUtility = new DateUtility();
            this.LogDAO = new LogDAO();
        }

        /// <summary>
        /// 取得查詢日期的Log列表(要先設定屬性DateStr)
        /// </summary>
        /// <returns>查詢結果的Log列表</returns>
        public IList<LogDO> Operate()
        {
            if (this.DateStr == null)
            {
                this.DateStr = this.DateUtility.GetStrToday();
            }
            Console.WriteLine(this.DateStr);
            IList<LogDO> logDOList = this.LogDAO.ListByDate(this.DateStr);
            return logDOList;
        }

        /// <summary>
        /// 取得查詢日期的Log列表
        /// </summary>
        /// <param name="dateStr">查詢日期</param>
        /// <returns>查詢結果的Log列表</returns>
        public IList<LogDO> Operate(string dateStr)
        {
            Console.WriteLine(this.DateStr);
            IList<LogDO> logDOList = this.LogDAO.ListByDate(dateStr);
            return logDOList;
        }

        /// <summary>
        /// 刪除log紀錄依據屬性(DateStr)的設定日期
        /// </summary>
        public void Delete()
        {
            if (this.DateStr == null)
            {
                this.DateStr = this.DateUtility.GetStrToday();
            }

            this.LogDAO.DeleteBefore(this.DateStr);
        }
    }
}
