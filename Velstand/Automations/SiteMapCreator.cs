using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using Umbraco.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Logging;
using Velstand.Constants;
using Velstand.Utilities;

namespace Velstand.Automations
{
    public static class SiteMapCreator
    {
        /// <summary>
        /// XMLサイトマップを生成する
        /// </summary>
        public static void GenerateXml()
        {
            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(
                                            System.Web.HttpContext.Current.Server.MapPath("/sitemap.xml"), Encoding.UTF8))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    // FIXME: 多言語化
                    var umbrachoHelper = new UmbracoHelper(UmbracoContext.Current);
                    Traverse(writer, umbrachoHelper.ContentSingleAtXPath("//VelstandRoot"));

                    writer.WriteEndElement();

                    writer.Flush();
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }
        }

        private static void Traverse(XmlTextWriter writer, IPublishedContent node)
        {
            var baseUrl = UrlUtil.SchemeAndAuthority();
            var items = node.Children.Where(w => !w.DocumentTypeAlias.StartsWith(VelstandPrefix.PrivateItem));
            if (items.Any())
            {
                foreach (var item in items)
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", baseUrl + item.Url);
                    // FIXME: 多言語化
                    writer.WriteElementString("lastmod", string.Format("{0:yyyy-MM-dd}", item.GetPropertyValue<DateTime>("releaseDate")) );
                    writer.WriteElementString("changefreq", "weekly");
                    writer.WriteElementString("priority", "0.50");
                    writer.WriteEndElement();
                    Traverse(writer, item);
                }
            }
        }
    }
}