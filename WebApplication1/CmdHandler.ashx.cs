using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWebSite
{
    /// <summary>
    /// CmdHandler 的摘要描述
    /// </summary>
    public class CmdHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            
            string cmd = string.Empty;
            string cmdIndex = "cmd";
            if (context.Request.Form[cmdIndex] != null)
            {
                cmd = context.Request.Form[cmdIndex];
            }
            else if (context.Request.QueryString[cmdIndex] != null)
            {
                cmd = context.Request.QueryString[cmdIndex];
            }
            //string cmd2 = context.Request.
            if (!String.IsNullOrEmpty(cmd) && cmd == "GiveMeFile")
            {
                
                this.WriteFile(context);
                //string path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\";
                //string fullPath = System.IO.Directory.GetFiles(path)[0];
                //System.IO.FileInfo file = new System.IO.FileInfo(fullPath);
                //context.Response.TransmitFile(fullPath, 0, file.Length);
                //context.Response.Write("Send file Success!");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Hello World");
                context.Response.Flush();
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            //HttpResponse rsp = new HttpResponse();
        }

        /// <summary>
        /// 取得小部分中的資料，然後將資料移至輸出資料流，以供下載
        /// ref:https://support.microsoft.com/zh-tw/kb/812406/zh-tw
        /// 要送出的測試檔案放在App_Data資料夾內
        /// </summary>
        /// <param name="context">HttpContext</param>
        private void WriteFile(HttpContext context)
        {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk:
            byte[] buffer = new byte[10000];

            // Length of the file:
            int length;

            // Total bytes to read:
            long dataToRead;

            // Identify the file to download including its path.
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = path + "App_Data\\";//"Programming JavaScript Applications.pdf";

            // Identify the file name.
            string fullPath = System.IO.Directory.GetFiles(filePath)[0];
            string fileName = System.IO.Path.GetFileName(fullPath);
            
            try
            {
                //open file
                iStream = new System.IO.FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                // Total bytes to read:
                dataToRead = iStream.Length;
                
                context.Response.Clear();
                context.Response.ContentType = "application/octet-stream";
                
                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                //read the bytes
                while (dataToRead > 0)
                {
                    // Verify that the client is connected.
                    if (context.Response.IsClientConnected)
                    {
                        // Read the data in buffer.
                        length = iStream.Read(buffer, 0, buffer.Length);

                        // Write the data to the current output stream.
                        context.Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the HTML output.
                        context.Response.Flush();

                        //clear buffer
                        buffer = new byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                // Trap the error, if any.
                context.Response.Write("Error:" + ex.StackTrace);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                
                context.Response.Status = "200";
                //close client connection
                context.Response.Close();
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
}