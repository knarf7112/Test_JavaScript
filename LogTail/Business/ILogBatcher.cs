using LogTail.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogTail.Business
{
    public interface ILogBatcher
    {
        string DateStr { get; set; }
        /// <summary>
        /// Dump log from log table
        /// </summary>
        /// <returns></returns>
        IList<LogDO> Operate();
        void Delete();
    }
}
