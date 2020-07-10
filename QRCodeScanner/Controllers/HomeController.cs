using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace QRCodeScanner.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult QRCodeScanner()
        {
            // TODO : I have generated QR code and stored image inside project directory(D:\My Projects\QRCodeScanner\QRCodeScanner\Content\images\QRCodeImage.png)
            // Also converted file path into url encoded format below but unfortunately it given 400-Bad Request error always. 
            
            // D%3A%5CMy%20Projects%5CQRCodeScanner%5CQRCodeScanner%5CContent%5Cimages%5CQRCodeImage.png
            
            // ViewData["QRCodeImageText"] = ScanQRCode("D%3A%5CMy%20Projects%5CQRCodeScanner%5CQRCodeScanner%5CContent%5Cimages%5CQRCodeImage.png"); // Not Working

            // I tried using same web url for my new QR code and its working as expected.
            ViewData["QRCodeImageText"] = ("http://api.qrserver.com/v1/create-qr-code/?data=This-is-QR-Code-Svanner-Sample-Application"); // Working

            return View();
        }

        [NonAction]
        public string ScanQRCode(string filePath)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.qrserver.com/v1/");

                //Http GET
                var responseTask = client.GetAsync("read-qr-code/?fileurl=" + filePath);
                responseTask.Wait();

                var result = responseTask.Result;

                string QRCodeJson = "Bad Request";
                if (result.IsSuccessStatusCode)
                {
                    QRCodeJson = result.Content.ReadAsStringAsync().Result;
                }

                return QRCodeJson;
            }
        }
    }
}