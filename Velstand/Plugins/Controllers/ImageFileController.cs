using System;
using System.Web;

using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Velstand.Plugins.Controllers
{
    [PluginController("Velstand")]
    [IsBackOffice]
    class ImageFileController : UmbracoAuthorizedJsonController
    {
        public string Get(HttpPostedFileBase file)
        {
            return "text";
        }
    }
}
