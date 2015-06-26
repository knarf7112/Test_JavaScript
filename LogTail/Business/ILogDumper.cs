using LogTail.Domain.Entity;
using System.Collections.Generic;

namespace LogTail.Business
{
    public interface ILogDumper
    {
        /// <summary>
        /// Dump log from log table
        /// </summary>
        /// <returns></returns>
        IList<LogDO> Operate();
    }
}
