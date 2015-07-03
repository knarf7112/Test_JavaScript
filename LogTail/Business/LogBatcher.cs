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
