using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NServiceBus.ServiceIgnition.Web.Controllers
{
    using System.IO;
    using System.Net;
    using System.Text;

    public class HomeController : Controller
    {
        private List<VersionConfigurationOptions> _availableOptions =
                new List<VersionConfigurationOptions>()
                {
                    new ConfigurationOptionBuilder_V5().GetConfigurationOptions(),
                };

        private List<IBuildBootstrappedSolutions> _bootstrappers =
                new List<IBuildBootstrappedSolutions>()
                {
                    new BootstrappedSolutionBuilder_V5(),
                };

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
                AvailableVersions = _availableOptions
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string Bootstrap(SolutionConfiguration configuration)
        {
            var bootstrapper = _bootstrappers.Single(i => i.Version == configuration.NServiceBusVersion);

            var solutionSaver = new SolutionSaver(savePath: HttpContext.Server.MapPath("~/GeneratedSolutions/"), nugetExePath: HttpContext.Server.MapPath("~/NuGet.exe"));

            var zipFile = solutionSaver.CreateSolution(bootstrapper, configuration);

            var parts = zipFile.Split('\\');

            var guid = parts[parts.Length - 2];

            return guid;
        }

        [HttpGet]
        public FileResult SolutionZip(string guid)
        {
            var path = HttpContext.Server.MapPath("~/GeneratedSolutions/" + guid + "/");

            var zipFile = Directory.GetFiles(path).Single(f => f.EndsWith(".zip"));

            var fileBytes = System.IO.File.ReadAllBytes(zipFile);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = Path.GetFileName(zipFile),
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(fileBytes, "application/zip");
        }
    }
}