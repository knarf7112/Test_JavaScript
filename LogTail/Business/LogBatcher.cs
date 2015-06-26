using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogTail.Business
{
    public class LogBatcher : ILogBatcher
    {
        public string DateStr { get; set; }
        

        public IList<Domain.Entity.LogDO> Operate()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
