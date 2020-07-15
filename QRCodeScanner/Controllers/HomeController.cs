using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

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
            var generatedQRCodeImage = GenerateQRCode("Example");

            ViewData["QRCodeImageText"] = ScanQRCode(generatedQRCodeImage);

            return View();
        }

        [NonAction]
        public string ScanQRCode(Stream qrCodeImage)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.qrserver.com/v1/");

                MultipartFormDataContent content = new MultipartFormDataContent();

                //content.Add(new ByteArrayContent(qrCodeImage, 0, qrCodeImage.Length), "file", "qrCodeImage.jpg");

                var imageContent = new StreamContent(qrCodeImage);

                content.Add(imageContent, "file");

                var responseTask = client.PostAsync("read-qr-code", content);
                responseTask.Wait();

                string QRCodeJson = "Bad Request";
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    QRCodeJson = responseTask.Result.Content.ReadAsStringAsync().Result;
                }

                return QRCodeJson;
            }
        }


        [NonAction]
        public Stream GenerateQRCode(string qrCodeText)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.qrserver.com/v1/");

                //Http GET
                var responseTask = client.GetAsync("create-qr-code/?size=150x150&data=" + qrCodeText);
                responseTask.Wait();

                var result = responseTask.Result;

                Stream generatedImage = null;
                if (result.IsSuccessStatusCode)
                {
                    generatedImage = result.Content.ReadAsStreamAsync().Result;
                }

                return generatedImage;
            }
        }
    }
}