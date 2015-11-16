using System.Collections.Generic;
using System.Web.Mvc;

namespace NServiceBus.ServiceIgnition.Web.Controllers
{
    using System.IO;
    using System.Net;
    using System.Text;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Documentation(string q)
        {
            var urlAddress = "http://docs.particular.net/search?q=" + q.Replace(" ", "+");
            var request = (HttpWebRequest)WebRequest.Create(urlAddress);
            using (var response = (HttpWebResponse)request.GetResponse())
            {

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var receiveStream = response.GetResponseStream())
                    {
                        StreamReader readStream;

                        if (response.CharacterSet == null)
                        {
                            readStream = new StreamReader(receiveStream);
                        }
                        else
                        {
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        }

                        using (readStream)
                        {
                            return readStream.ReadToEnd();
                        }
                    }
                }
            }

            return "";
        }

        [HttpGet]
        public JsonResult BootstrapBuilder()
        {
            var model = new BootstrapperOptions()
            {
                AvailableVersions = new List<VersionConfigurationOptions>()
                {
                    new ConfigurationOptionBuilder_V5().GetConfigurationOptions(),
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}