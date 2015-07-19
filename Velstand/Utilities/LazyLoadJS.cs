using System;
using System.Web;
using System.Linq;
using HtmlAgilityPack;
using Umbraco.Core.Models;

namespace Velstand.Utilities
{
    public static class LazyLoadJS
    {
        /// <summary>
        /// HtmlString内のimgタグをlazyload化する
        /// </summary>
        /// <param name="self">HtmlString</param>
        /// <param name="appendClass">css class</param>
        /// <param name="loadingImg">loading時の画像パス</param>
        /// <returns>HtmlString</returns>
        public static IHtmlString ToLazyLoad(this IHtmlString self, string appendClass = "responsive-img", string loadingImg = "/Content/velstand/loading.gif")
        {
            try
            {
                var html = new HtmlDocument();
                html.LoadHtml(self.ToString());
                foreach (var imgTag in html.DocumentNode.DescendantsAndSelf().Where(x => x.Name == "img"))
                {
                    imgTag.SetAttributeValue("data-original", imgTag.Attributes["src"].Value);
                    imgTag.SetAttributeValue("class", "lazy-load " + appendClass);
                    imgTag.SetAttributeValue("src", loadingImg);
                }
                return new HtmlString(html.DocumentNode.InnerHtml);
            }
            catch
            {
                return new HtmlString(string.Empty);
            }
        }
    }
}
