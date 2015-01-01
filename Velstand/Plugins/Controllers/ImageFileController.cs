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
            var files = HttpContext.Current.Request.Files;
            if(files.Count > 0)
            {
                var imageModel = new ImageFileModel((HttpPostedFile)files[0], "/media/velstand");
                imageModel.Save();
                return imageModel.FilePath;
            }
            return string.Empty;
        }

        public string Get(HttpPostedFile file)
        {
            var imageModel = new ImageFileModel(file, "/media/velstand");
            //if (!imageModel.Save())
            //{
                throw new Exception("error please lookup logfile");
            //}

            return imageModel.FilePath;
        }
    }
}
