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

namespace MyWebSite
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
                    cmdObj = JsonConvert.DeserializeObject<CmdObject>(requestJsonString);
                }
            }
            else if (context.Request.HttpMethod.ToUpper() == "GET")
            {
                cmdObj = new CmdObject()
                {
                    cmdType = context.Request.QueryString["cmdType"],
                    cmdData = context.Request.QueryString["cmdData"]
                };
                
            }
            else
            {
                return;
            }
            if (cmdObj != null)
                DoResponse(context, cmdObj);
        }

        private void DoResponse(HttpContext context,CmdObject obj)
        {
            if (obj.cmdType == "GetLog")
            {
                int lastId = -1;
                if (int.TryParse(obj.cmdData, out lastId))
                {
                    IList<LogDO> logList = Global.logDumper.Operate(ref lastId);
                    string logListJsonString = JsonConvert.SerializeObject(logList);
                    Console.WriteLine("轉完的JSON資料:" + logListJsonString);
                    context.Response.AppendHeader("Content-Type", "application/json");
                    context.Response.Write(logListJsonString);
                }
                else
                {
                    //轉換失敗,前端傳來的cmdData有問題
                    //Error log 之後再補...
                }
            }
            else if (obj.cmdType == "GetBankStatus")
            {
                //TODO...
            }
            else
            {

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
    }
}