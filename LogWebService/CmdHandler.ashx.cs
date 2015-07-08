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
            if (!string.IsNullOrEmpty(cmdObj.cmdType))
                DoResponse(context, cmdObj);
        }

        private void DoResponse(HttpContext context,CmdObject obj)
        {
            if (obj.cmdType == "GetLog")
            {
                int lastId = -1;
                if (obj.lastId >= -1)
                {
                    lastId = obj.lastId;
                    IList<LogDO> logList = Global.logDumper.Operate(ref lastId);
                    IList<LogDO> resultList = null;
                    int startItem = 0;
                    int getListCount = 20;
                    context.Response.AppendHeader("Content-Type", "application/json; charset=utf-8");//沒設定charset到前端Run此頁轉碼時中文會變亂碼
                    //response loop a part
                    while (startItem < logList.Count)
                    {
                        resultList = logList.Skip(startItem).Take(getListCount).ToList();
                        startItem += getListCount;
                        string logListJsonString = JsonConvert.SerializeObject(resultList);
                        //string filterString = logListJsonString.Replace("\\", "");

                        //var i = context.Response.OutputStream.WriteTimeout;//這個資料流不支援逾時。
                        try
                        {
                            //byte[] responseBytes = Encoding.GetEncoding(950).GetBytes(logListJsonString);//使用"Big5"編碼中文就不會亂碼了
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
                    //CmdObject responseObj = new CmdObject()
                    //{
                    //    cmdType = "ResponseLog",
                    //    cmdData = logListJsonString,
                    //    lastId = lastId,
                    //};
                    //string responseString = JsonConvert.SerializeObject(responseObj);
                    //context.Response.Write(responseString);
                    //context.Response.Flush();
                }
                //else
                //{
                //    //轉換失敗,前端傳來的cmdData有問題
                //    //Error log 之後再補...
                //}
            }
            else if (obj.cmdType == "GetLogByDate")
            {
                Global.logBatcher.DateStr = obj.searchDate;
                IList<LogDO> logList = Global.logBatcher.Operate();
                string logListJsonString = JsonConvert.SerializeObject(logList);
                Console.WriteLine("轉完的JSON資料:" + logListJsonString);
                context.Response.AppendHeader("Content-Type", "application/json");
                context.Response.Write(logListJsonString);
            }
            else if (obj.cmdType == "GetBankStatus")
            {
                //TODO...
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

    public class CmdObject
    {
        
        public string cmdType { get; set; }

        public string cmdData { get; set; }

        public int lastId { get; set; }

        public string searchDate { get; set; }
    }
}