using System;
//
using System.Web.Caching;

namespace MyWebSite
{
    public class Global : System.Web.HttpApplication
    {
        Cache qq;
        protected void Application_Start(object sender, EventArgs e)
        {
            //qq = new Cache(); 
            //Test_JavaScript.App_Code.TestRun tt = new App_Code.TestRun(){Name = "Qoo", Age = 18, IsRun = true};
            //qq.Add("test", tt, new CacheDependency(""), DateTime.Now, new TimeSpan(0, 0, 20), CacheItemPriority.Default, new CacheItemRemovedCallback((string qq2, object obj, CacheItemRemovedReason oo) => {
            //    var ss = qq2;

            //}));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var ss = this.Context.Request.QueryString["test"];
            //this.Context.Response.ContentType = "text/html";
            //this.Context.Response.Write(@"Test_quote.html");
            //this.Context.Response.ClearContent();
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