using System;
//
using LogTail.Common;

namespace LogTail.Domain.Entity
{
    /// <summary>
    /// 存放Log記錄用的資料物件(可序列化)
    /// </summary>
    [Serializable]
    public class LogDO : AbstractDO
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Thread { get; set; }
        public virtual string Level { get; set; }
        public virtual string Logger { get; set; }
        public virtual string Message { get; set; }
        public virtual string Exception { get; set; }
        public virtual string HostId { get; set; }
    }
}
