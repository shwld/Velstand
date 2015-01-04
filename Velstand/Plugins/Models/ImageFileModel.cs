using System;
using System.Web;
using System.IO;
using System.Reflection;
using Umbraco.Core.Logging;
using Velstand.Utilities;

namespace Velstand.Plugins.Models
{
    public class ImageFileModel
    {
        private HttpPostedFile postedFile;
        private string saveFolder = "/media/Images";
        public string FilePath;
        public ImageFileModel(HttpPostedFile file, string folderPath = "/media/images", string extention = "png")
        {
            this.postedFile = file;

            this.saveFolder = folderPath + "/" + Generator.CurrentTimeStamp();
            this.FilePath = string.Format("{0}/{1}.{2}",
                                    this.saveFolder,
                                    Path.GetFileName(file.FileName),
                                    extention);
        }

        public Boolean IsValid()
        {
            if (this.postedFile == null) { return false; }
            if (!this.postedFile.ContentType.StartsWith("image/")) { return false; }

            return true;
        }

        public Boolean Save()
        {
            if (!this.IsValid()) { return false; }
            try
            {
                Generator.CreateDirectory(this.saveFolder);

                string physicalPath = HttpContext.Current.Server.MapPath(this.FilePath);
                this.postedFile.SaveAs(physicalPath);
            }
            catch(Exception ex)
            {
                LogHelper.Error(MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
                return false;
            }
            return true;
        }
    }
}
