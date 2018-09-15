using Rotativa;
using Rotativa.Options;
using RotativaPDF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RotativaPDF.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>第一種透過 View 產生PDF</summary>
        /// <returns></returns>
        public ActionResult DownloadPDF()
        {
            /*透過View 產生PDF*/
            //透過DB或是MODEL指定PDF內容
            GeneratePDFModel model = new GeneratePDFModel();
            string content = "<h2>Rotativa PDF From ViewAsPdf <h2>" +
                             "<p>Rotativa真的是太方便了!!! <p>";
            string logoFilePath = @"/Images/DotBlogsLogo.gif";
            //指定PDF內容
            model.PDFContent = content;
            //圖片位置
            model.PDFLogo = Server.MapPath(logoFilePath);


            /*產生PDF*/
            ViewAsPdf GenPDF = new ViewAsPdf();
            //指定檔案名稱
            GenPDF.FileName = "ViewPdf.pdf";
            //將GeneratePDFModel 所設定的內容置入
            GenPDF.Model = model;
            //對應View GeneratePDF.cshtml
            GenPDF.ViewName = "GeneratePDF";
            //設定Margin
            GenPDF.PageMargins = new Margins() { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            //指定大小預設為A4
            GenPDF.PageSize = Size.A4;
            return GenPDF;
        }

        /// <summaroy>第二種透過 ACTIO 產生PDF</summary>
        /// <returns></returns>
        public ActionResult DownloadActionAsPDF()
        {
            /*透過ACTION產生PDF*/
            //對應ACTION 名稱 GeneratePDF()
            ActionAsPdf GenPdf = new ActionAsPdf("ActionGeneratePDF");
            GenPdf.FileName = "ActionAsPdf.pdf";

            return GenPdf;
        }
        /// <summary>ActionGeneratePDF</summary>
        /// <returns></returns>
        public ActionResult ActionGeneratePDF()
        {
            GeneratePDFModel model = new GeneratePDFModel();

            string content = "<h2>Rotativa PDF From ActionAsPdf<h2>" +
                             "<p>透過Action產生PDF<p>";
            string logoFilePath = @"/Images/DotBlogsLogo.gif";

            model.PDFContent = content;
            model.PDFLogo = Server.MapPath(logoFilePath);

            return View(model);
        }

        /// <summary>第三種透過 PartialView 產生PDF</summary>
        /// <returns></returns>
        public ActionResult DownloadPartialViewPDF()
        {
            GeneratePDFModel model = new GeneratePDFModel();

           
            string content = "<h2>PDF Created<h2>" +
            "<p>透過partialView產生<p>";
            string logoFilePath = @"/Images/DotBlogsLogo.gif";

            model.PDFContent = content;
            model.PDFLogo = Server.MapPath(logoFilePath);
            /*透過PartialView 產生PDF*/
            PartialViewAsPdf PdfPartial = new PartialViewAsPdf();
            //指定PDF內容
            PdfPartial.Model = model;
            //指定檔名
            PdfPartial.FileName = "partialViewAsPdf.pdf";
            //指定對應的partialView~_PartialViewTest.cshtml
            PdfPartial.ViewName = "_PartialViewTest";
            //設定Margin
            PdfPartial.PageMargins = new Margins() { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            //指定大小預設為A4
            PdfPartial.PageSize = Size.A4;

            return PdfPartial;
        }



        /// <summary> 第四種透過網址產生PDF</summary>
        /// <returns></returns>
        public ActionResult UrlAsPDF()
        {
            //透過URL位置產生PDF
            UrlAsPdf GenUrlPDF = new UrlAsPdf("https://tw.news.yahoo.com/sports/");
            //設定檔案名稱
            GenUrlPDF.FileName = "dotblogs_url_test.pdf";
            //設定Margin
            GenUrlPDF.PageMargins = new Margins() { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            //指定大小預設為A4
            GenUrlPDF.PageSize = Size.A4;

            ////BuildPdf此方法提供使用者再SEVER端產出PDF
            //byte[] GenPDFinSever = GenUrlPDF.BuildPdf(this.ControllerContext);
            ////存入Server端的位置
            //System.IO.File.WriteAllBytes(@"D:\hello.pdf", GenPDFinSever);

            return GenUrlPDF;
        }
       
        /// <summary>第五種透過BuildPdf方法再Server端產生PDF </summary>
        /// <returns></returns>
        public JsonResult LocalPDFGen()
        {
            //透過URL位置產生PDF
            UrlAsPdf GenUrlPDF = new UrlAsPdf("https://tw.news.yahoo.com/sports/");
            //設定檔案名稱
            GenUrlPDF.FileName = "dotblogs_url_test.pdf";
            //設定Margin
            GenUrlPDF.PageMargins = new Margins() { Top = 10, Bottom = 10, Left = 10, Right = 10 };
            //指定大小預設為A4
            GenUrlPDF.PageSize = Size.A4;

            //BuildPdf此方法提供使用者再SEVER端產出PDF
            //需綁定再System.Web.Mvc.ControllerContext之下
            byte[] GenPDFinSever = GenUrlPDF.BuildPdf(this.ControllerContext);
            //存入Server端的位置
            string SavePath = GenUrlPDF.WkhtmltopdfPath + @"\" + GenUrlPDF.FileName;
            //產生PDF檔
            System.IO.File.WriteAllBytes(SavePath, GenPDFinSever);

            RtvMsg Rtv = new RtvMsg();
            if (System.IO.File.Exists(SavePath))
            {
                string UrlPath = @"http://" + Request.Url.Authority + "/Rotativa/" + GenUrlPDF.FileName;
                Rtv.Path = UrlPath;
                Rtv.Message = "已產生檔案";
            }
            else
            {
                Rtv.Path = "";
                Rtv.Message = "檔案未產生";

            }


            return Json(Rtv, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary> LocalPDFGen回傳訊息物件
        /// </summary>
        private class RtvMsg
        {
            public string Path { get; set; }
            public string Message { get; set; }
        }


       
    }
}