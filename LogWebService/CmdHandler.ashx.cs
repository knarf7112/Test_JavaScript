using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using LogTail.Domain.Entity;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace LogWebService
{
    /// <summary>
    /// CmdHandler 的摘要描述
    /// </summary>
    public class CmdHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            #region 取得前端Command物件
            //command object
            CmdObject cmdObj = null;
            if (context.Request.HttpMethod.ToUpper() == "POST")
            {
                context.Request.InputStream.Position = 0;
                using (StreamReader sr = new StreamReader(context.Request.InputStream, Encoding.UTF8))
                {
                    string requestJsonString = sr.ReadToEnd();
                    try
                    {
                        cmdObj = JsonConvert.DeserializeObject<CmdObject>(requestJsonString);
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write("<h1>前端傳來的命令物件錯誤!</h1>");
                    }

                }
            }
            else if (context.Request.HttpMethod.ToUpper() == "GET")
            {
                cmdObj = new CmdObject()
                {
                    cmdType = context.Request.QueryString["cmdType"] != null ? context.Request.QueryString["cmdType"].ToString() : "",
                    cmdData = context.Request.QueryString["cmdData"] != null ? context.Request.QueryString["cmdData"].ToString() : "",
                    lastId = context.Request.QueryString["lastId"] != null ? Convert.ToInt32(context.Request.QueryString["lastId"]) : -1,
                    searchDate = context.Request.QueryString["searchDate"] != null ? context.Request.QueryString["searchDate"].ToString() : ""
                };
                
            }
            else
            {
                return;
            }
            #endregion
            
            if (!string.IsNullOrEmpty(cmdObj.cmdType))
            {
                IList<LogDO> logList = GetDataByCmd(cmdObj);
                WriteResponseByPartial<LogDO>(context, logList);
            }
        }

        /// <summary>
        /// 依據前端Command取得log紀錄列表
        /// </summary>
        /// <param name="obj">前端Command</param>
        /// <returns>log紀錄列表</returns>
        private IList<LogDO> GetDataByCmd(CmdObject obj)
        {
            if (obj.cmdType == "GetLog")
            {
                #region 依據起始Id取得起始Id之後的Log紀錄
                int lastId = -1;
                if (obj.lastId >= -1)
                {
                    lastId = obj.lastId;
                    IList<LogDO> logList = Global.logDumper.Operate(ref lastId);
                    return logList;
                    #region (舊的)寫在裡面測試用
                    //IList<LogDO> resultList = null;
                    //int startItem = 0;
                    //int getListCount = 20;
                    //context.Response.AppendHeader("Content-Type", "application/json; charset=utf-8");//沒設定charset到前端Run此頁轉碼時中文會變亂碼
                    ////response loop a part
                    //while (startItem < logList.Count)
                    //{
                    //    resultList = logList.Skip(startItem).Take(getListCount).ToList();
                    //    startItem += getListCount;
                    //    string logListJsonString = JsonConvert.SerializeObject(resultList);
                    //    //string filterString = logListJsonString.Replace("\\", "");

                    //    //var i = context.Response.OutputStream.WriteTimeout;//這個資料流不支援逾時。
                    //    try
                    //    {
                    //        //byte[] responseBytes = Encoding.GetEncoding(950).GetBytes(logListJsonString);//使用"Big5"編碼中文就不會亂碼了(IE only)
                    //        byte[] responseBytes = Encoding.UTF8.GetBytes(logListJsonString);
                    //        context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                    //        //context.Response.Write("國字測試:會亂碼嗎?" + logListJsonString);
                    //        //context.Response.OutputStream.Flush();//看不出來有推送前台,但是確定傳超過45mb的資料量也不會crash
                    //        context.Response.Flush();//強制將資料推送出去
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        context.Response.Write("<h1>資料流推送錯誤!" + ex.StackTrace + "</h1>");
                    //        return;
                    //    }
                    //}
                    #endregion
                }
                //else
                //{
                //    //轉換失敗,前端傳來的cmdData有問題
                //    //Error log 之後再補...
                //}
                #endregion
            }
            else if (obj.cmdType == "GetLogByDate")
            {
                #region 依據日期取得此日期的Log紀錄
                IList<LogDO> logList = Global.logBatcher.Operate(obj.searchDate);
                return logList;
                #endregion
            }
            else if (obj.cmdType == "GetBankStatus")
            {
                //TODO...
                return default(IList<LogDO>);
            }
            return default(IList<LogDO>);
        }

        /// <summary>
        /// 將資料拆分成一小部分並分批來response回去
        /// </summary>
        /// <typeparam name="T">poco型別</typeparam>
        /// <param name="context">HttpContext</param>
        /// <param name="logList">資料列表</param>
        /// <param name="partListCount">每次要推送的筆數</param>
        private void WriteResponseByPartial<T>(HttpContext context,IList<T> logList,int partListCount = 20)
        {
            IList<T> resultList = null;
            int startItem = 0;
            int getListCount = partListCount;
            context.Response.AppendHeader("Content-Type", "application/json; charset=utf-8");//沒設定charset到前端Run此頁轉碼時中文會變亂碼
            //response loop a part 使用部分資料推送方式
            while (startItem < logList.Count)
            {
                resultList = logList.Skip(startItem).Take(getListCount).ToList();
                startItem += getListCount;
                string logListJsonString = JsonConvert.SerializeObject(resultList);
                //string filterString = logListJsonString.Replace("\\", "");

                //var i = context.Response.OutputStream.WriteTimeout;//這個資料流不支援逾時。
                try
                {
                    //byte[] responseBytes = Encoding.GetEncoding(950).GetBytes(logListJsonString);//使用"Big5"編碼中文就不會亂碼了(IE only)
                    byte[] responseBytes = Encoding.UTF8.GetBytes(logListJsonString);
                    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                    //context.Response.Write("國字測試:會亂碼嗎?" + logListJsonString);
                    //context.Response.OutputStream.Flush();//看不出來有推送前台,但是確定傳超過45mb的資料量也不會crash
                    context.Response.Flush();//強制將資料推送出去
                }
                catch (Exception ex)
                {
                    context.Response.Write("<h1>資料流推送錯誤!" + ex.StackTrace + "</h1>");
                    return;
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 前端命令物件poco
    /// </summary>
    public class CmdObject
    {
        
        public string cmdType { get; set; }

        public string cmdData { get; set; }

        public int lastId { get; set; }

        public string searchDate { get; set; }
    }
}