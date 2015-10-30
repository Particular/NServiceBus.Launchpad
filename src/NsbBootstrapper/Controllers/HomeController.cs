using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NsbBootstrapper.Models;

namespace NsbBootstrapper.Controllers
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

        private ConfigurationItem<T> MakeConfigurationItem<T>(T item) where T : struct, IConvertible
        {
            return new ConfigurationItem<T>()
            {
                Name = item.ToString(),
                Value = item
            };   
        }

        private List<ConfigurationItem<T>> MakeConfigurationList<T>(params T[] values) where T : struct, IConvertible
        {
            return values.Select(MakeConfigurationItem).ToList();
        }

        [HttpGet]
        public string Documentation(string q)
        {
            var urlAddress = "http://docs.particular.net/search?q=" + q.Replace(" ", "+");
            var request = (HttpWebRequest) WebRequest.Create(urlAddress);
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
            var version5 = new VersionBuilderModel()
            {
                NServiceBusVersion = NServiceBusVersion.Five,
            };

            AddDefaultOptions(version5);

            var version6 = new VersionBuilderModel()
            {
                NServiceBusVersion = NServiceBusVersion.Six,
            };

            AddDefaultOptions(version6);

            var model = new BootstrapBuilderModel()
            {
                AvailableVersions = new List<VersionBuilderModel>()
                {
                    version5,
                    version6
                }
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private void AddDefaultOptions(VersionBuilderModel version)
        {
            version.AvailablePersistence =
                MakeConfigurationList(
                    Persistence.None,
                    Persistence.Msmq,
                    Persistence.NHibernate,
                    Persistence.RavenDB,
                    Persistence.AzureStorage);

            version.AvailableSerializers =
                MakeConfigurationList(
                    Serializer.Json,
                    Serializer.Xml,
                    Serializer.Binary);

            version.AvailableTransports =
                MakeConfigurationList(
                    Transport.AzureServiceBus,
                    Transport.Msmq,
                    Transport.RabbitMQ,
                    Transport.SqlServer);
        }
    }
}