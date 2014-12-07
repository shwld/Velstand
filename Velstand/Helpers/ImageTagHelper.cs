using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Core.Models;
using Velstand.Constants;
using Velstand.Models;

namespace Velstand.Helpers
{
    public static class ImageTagHelper
    {
        /// <summary>
        /// ImageCropから画像のパスを取得する
        /// HACK:パフォーマンスが問題なければLazyLoadJSクラスを使いたい
        /// </summary>
        /// <param name="alias">ImageCropプロパティのエイリアス</param>
        /// <param name="targetSize">Cropサイズのエイリアス</param>
        /// <returns>imgタグ(IHtmlString)</returns>
        public static IHtmlString Crop(this HtmlHelper helper, IPublishedContent content, string alias, string targetSize, string classValue = "img-responsive", bool isLazyLoad = true, string titleProperty = "title")
        {
            try
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var result = content.GetCropUrl(alias, targetSize);
                if (isLazyLoad)
                {
                    return new HtmlString(
                                        string.Format("<img src={0} alt={1} data-original={2} class=\"lazy-load {3}\"/>",
                                                              "/Content/velstand/loading.gif",
                                                              umbracoHelper.Field(content, "title"),
                                                              result,
                                                              classValue));
                }
                else
                {
                    return new HtmlString(
                                        string.Format("<img src={0} alt={1} class=\"{2}\"/>",
                                                             result,
                                                             umbracoHelper.Field(content, "title"),
                                                             classValue));
                }
            }
            catch
            {
                return new HtmlString(string.Empty);
            }
        }
    }
}
