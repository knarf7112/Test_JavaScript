using LogTail.Domain.Entity;
using System.Collections.Generic;

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
