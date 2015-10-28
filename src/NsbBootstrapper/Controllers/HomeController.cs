using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NsbBootstrapper.Models;

namespace NsbBootstrapper.Controllers
{
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
                    Persistence.NHibernate,
                    Persistence.RavenDb,
                    Persistence.AzureStorage);

            version.AvailableSerializers =
                MakeConfigurationList(
                    Serializer.Json,
                    Serializer.Xml,
                    Serializer.Binary);

            version.AvailableTransports =
                MakeConfigurationList(
                    Transport.Azure,
                    Transport.Msmq,
                    Transport.RabbitMq,
                    Transport.Sql);
        }
    }
}