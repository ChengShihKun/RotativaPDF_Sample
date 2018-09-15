using Rotativa;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RotativaPDF.Controllers.API
{
  
    [RoutePrefix("api/GenPDF")]
    public class GenPDFController : ApiController
    {
        /// <summary> LocalPDFGen回傳訊息物件</summary>
        public class RtvMsg
        {
            public string Path { get; set; }
            public string Message { get; set; }
        }
        [Route("RotativaFun")]
        [HttpPost]
        public RtvMsg RotativaFun()
        {
                //欲轉換PDF的URL位置
                string pdfurl = "https://tw.news.yahoo.com/sports/";
                //指定檔案名稱
                string fileName = "FromApi.pdf";
                //選擇儲存Server端路徑
                string pdfTemp = System.Web.HttpContext.Current.Server.MapPath("~/Rotativa") + @"\" + fileName;
                //wkhtmltopdf.exe 路徑
                string wkhtmltopdfPath = System.Web.HttpContext.Current.Server.MapPath("~/Rotativa");
                //wkhtmltopdf.exe 欲執行的指令
                string Command = "--zoom 2 --margin-top 15mm --margin-bottom 15mm --margin-right 15mm --margin-left 15mm --page-size A4  ";
                //透過Rotativa所提供的靜態類別中的Convert執行轉換HTML TO PDF
                byte[] GenPdfByte = WkhtmltopdfDriver.Convert(wkhtmltopdfPath, Command + pdfurl);
                System.IO.File.WriteAllBytes(pdfTemp, GenPdfByte);
                RtvMsg Rtv = new RtvMsg();
                if (System.IO.File.Exists(pdfTemp))
                {

                    string UrlPath = @"http://" + this.Request.RequestUri.Authority + "/Rotativa/" + fileName;
                    Rtv.Path = UrlPath;
                    Rtv.Message = "已產生檔案";
                }
                else
                {
                    Rtv.Path = "";
                    Rtv.Message = "檔案未產生";

                }

                return Rtv;
        }

        [Route("EXEwkhtmltopdf")]
        [HttpPost]
        public RtvMsg EXEwkhtmltopdf()
        {
            
            RtvMsg Rtv = new RtvMsg();
        
            //欲轉換PDF的URL位置
            string pdfurl = "https://tw.news.yahoo.com/sports/";
            //指定檔案名稱
            string fileName = "FromApiEXEwkhtmltopdf.pdf";
            //選擇儲存Server端路徑
            string pdfTemp = System.Web.HttpContext.Current.Server.MapPath("~/Rotativa") + @"\" + fileName;
            //wkhtmltopdf.exe 路徑
            string wkhtmltopdfPath = System.Web.HttpContext.Current.Server.MapPath("~/Rotativa") + @"\wkhtmltopdf.exe";
            //wkhtmltopdf.exe 欲執行的指令
            string Command = "--zoom 2 --margin-top 15mm --margin-bottom 15mm --margin-right 15mm --margin-left 15mm --page-size A4  ";
      
            System.Diagnostics.ProcessStartInfo Pss = new ProcessStartInfo();
            Pss.FileName = wkhtmltopdfPath;//HTML轉PDF執行檔
            Pss.Arguments = string.Format("{0} {1} {2}", Command, pdfurl, pdfTemp);
            Pss.UseShellExecute = false;
            Pss.RedirectStandardInput = true;
            Pss.RedirectStandardOutput = true;
            using (System.Diagnostics.Process p = new Process())
            {
                p.StartInfo = Pss;
                p.Start();
                p.WaitForExit(60000);
                //判斷執行結果
                if (p.ExitCode == 0)
                {
                    p.Close();

                    if (System.IO.File.Exists(pdfTemp))
                    {

                        string UrlPath = @"http://" + this.Request.RequestUri.Authority + "/Rotativa/" + fileName;
                        Rtv.Path = UrlPath;
                        Rtv.Message = "已產生檔案";
                    } else{
                        Rtv.Path = "";
                        Rtv.Message = "檔案未產生";
                    }
                }
            }
            return Rtv;
        }
    }
}
