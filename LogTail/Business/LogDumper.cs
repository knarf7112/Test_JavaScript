using LogTail.Data;
using LogTail.Domain.Entity;
using System;
using System.Collections.Generic;

namespace LogTail.Business
{
    public class LogDumper : ILogDumper
    {
        public LogDAO logDAO { get; set; }

        private int lastId = -1;//搜尋用的起始Id
        public LogDumper()
        {
            this.logDAO = new LogDAO();
        }

        /// <summary>
        /// 取得Log列表(每次10筆)並記錄最後一筆log的Id
        /// </summary>
        /// <returns></returns>
        public IList<LogDO> Operate()
        {
            //first time
            if (lastId == -1)
            {
                //取得最大id
                int lastCnt = logDAO.GetMaxPk();//get the last Id
                //若Id是10以上,則起始的lastId = 最大PK - 10(確保有資料可取)
                if (lastCnt >= 10)
                {
                    this.lastId = lastCnt - 10;
                }
            }
            Console.WriteLine(lastId);
            //IList<string> logList = new List<string>();
            IList<LogDO> logDOList = this.logDAO.ListAfter(lastId);//dump log poco 取得的資料範圍(lastId ~ MaxId)
            int cnt = logDOList.Count;

            //若有log則修改lastId值(lastId即表示目前已Dump出log的指標(下次Dump時lastId就表示上次Dump的Id位置))
            if (cnt > 0)
            {
                lastId = logDOList[cnt - 1].Id;
            }
            return logDOList;
        }

        //
        public IList<LogDO> Operate(ref int lastID)
        {
            int startId = 0;
            //若lastID低於範圍內則從最後一筆資料往前取10筆
            if (lastID <= -1)
            {
                int lastCnt = this.logDAO.GetMaxPk();
                if (lastCnt >= 10)
                {
                    startId = lastCnt - 10;
                }
                else
                {
                    startId = lastCnt;
                }
            }
            else
            {
                startId = lastID;
            }
            Console.WriteLine(startId);

            IList<LogDO> logList = this.logDAO.ListAfter(startId);
            if (startId > 0)
            {
                lastID = logList[logList.Count - 1].Id;
            }
            return logList;
        }
    }
}
