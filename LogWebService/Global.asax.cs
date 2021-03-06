﻿using System;
//
using System.Web.Caching;
using System.Web.SessionState;
using System.Collections.Generic;
//
using LogTail.Business;

namespace LogWebService
{
    public class Global : System.Web.HttpApplication
    {
        //Cache qq;
        public static LogDumper logDumper { get; set; }

        public static ILogBatcher logBatcher { get; set; }

        /// <summary>
        /// 因為沒有Session物件,模擬Session用的字典檔
        /// </summary>
        public static Dictionary<String,object> _MySession;
        protected void Application_Start(object sender, EventArgs e)
        {
            //被IIS執行起動的第一次進入點
            _MySession = new Dictionary<string, object>();

            this.Error += Global_Error;//好像沒用
            logDumper = new LogDumper();
            logBatcher = new LogBatcher();
            //qq = new Cache(); 
            //Test_JavaScript.App_Code.TestRun tt = new App_Code.TestRun(){Name = "Qoo", Age = 18, IsRun = true};
            //qq.Add("test", tt, new CacheDependency(""), DateTime.Now, new TimeSpan(0, 0, 20), CacheItemPriority.Default, new CacheItemRemovedCallback((string qq2, object obj, CacheItemRemovedReason oo) => {
            //    var ss = qq2;

            //}));
            
            
        }

        //
        void Global_Error(object sender, EventArgs e)
        {
            //var ss = ((Global)sender);

            this.Response.Write("Server端有問題:執行拋出的異常");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //這是網頁發出的Request進入點
            if (logDumper == null)
            {
                logDumper = new LogDumper();
            }
            if (logBatcher == null)
            {
                logBatcher = new LogBatcher();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}