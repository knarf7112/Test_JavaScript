using LogTail.Data;
using LogTail.Domain.Entity;
using System.Collections.Generic;

namespace LogTail.Business
{
    public class LogDumper : ILogDumper
    {
        public LogDAO logDAO { get; set; }

        public IList<LogDO> Operate()
        {
            throw new System.NotImplementedException();
        }
    }
}
