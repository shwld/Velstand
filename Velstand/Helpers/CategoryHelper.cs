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
    public static class CategoryHelper
    {
        /// <summary>
        /// MultiNodePickerのプロパティを指定して、カテゴリー検索のURLを取得する
        /// </summary>
        /// <param name="content">IPublishedContent</param>
        /// <param name="propertyAlias">カテゴリーのエイリアス</param>
        /// <param name="namePropertyAlias">カテゴリー名のエイリアス</param>
        /// <param name="separator">セパレーター</param>
        /// <returns>IHtmlString</returns>
        public static IHtmlString CategoryWithLink(this HtmlHelper helper, IPublishedContent content, string propertyAlias = "category", string separator = ", ")
        {
            var links = content.VMultiNodes(propertyAlias)
                                          .Select(x => string.Format("<a href=\"{0}\" >{1}</a>",
                                                                            content.VHolderTop().VCategoryUrl(x.Id),
                                                                            x.Name));
            return new HtmlString(string.Join(separator, links));
        }
    }
}
