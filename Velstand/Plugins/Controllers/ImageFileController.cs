using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Velstand.Plugins.Models;

namespace Velstand.Plugins.Controllers
{
    [PluginController("Velstand")]
    [IsBackOffice]
    public class ImageFileController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public string Post()
        {
            var request = HttpContext.Current.Request;
            var files = request.Files;
            if(files.Count > 0)
            {
                var imageModel = new ImageFileModel((HttpPostedFile)files[0], request["folderPath"], request["fileName"]);
                imageModel.Save();
                return imageModel.FilePath;
            }
            return string.Empty;
        }
    }
}
