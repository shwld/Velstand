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
    public static class DateFormatHelper
    {

        /// <summary>
        /// 数字文字列から日付文字列を取得する
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IHtmlString BreadCrumbDateFormat(this HtmlHelper helper, string dateNumber)
        {
            try
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var result = string.Empty;
                int dateInt;
                if (!int.TryParse(dateNumber, out dateInt))
                {
                    return new HtmlString(string.Empty);
                }
                switch (dateNumber.Length)
                {
                    case (4):
                        result = string.Format(
                                                    umbracoHelper.GetDictionaryValue("BreadCrumbYear"),
                                                    new DateTime(int.Parse(dateNumber.Substring(0, 4)), 1, 1));
                        break;
                    case (6):
                        result = string.Format(
                                                    umbracoHelper.GetDictionaryValue("BreadCrumbMonth"),
                                                    new DateTime(int.Parse(dateNumber.Substring(0, 4)), int.Parse(dateNumber.Substring(4, 2)), 1));
                        break;
                    case (8):
                        result = string.Format(
                                                    umbracoHelper.GetDictionaryValue("BreadCrumbDay"),
                                                    new DateTime(int.Parse(dateNumber.Substring(0, 4)), int.Parse(dateNumber.Substring(4, 2)), int.Parse(dateNumber.Substring(6, 2))));
                        break;
                    default:
                        break;
                }

                return new HtmlString(result);
            }
            catch
            {
                return new HtmlString(string.Empty);
            }
        }

        /// <summary>
        /// 翻訳辞書を使い公開日付のフォーマットを行う
        /// </summary>
        /// <param name="content">IPublishedContent</param>
        /// <param name="alias">日付パラメータのエイリアス</param>
        /// <param name="dictionalyKey">DictionalyのKey</param>
        /// <returns>IHtmlString</returns>
        public static IHtmlString DictionalyDateFormat(this HtmlHelper helper, IPublishedContent content, string alias, string dictionalyKey = "DateFormat")
        {
            var umbHelper = new UmbracoHelper(UmbracoContext.Current);
            try
            {
                return new HtmlString(
                                       string.Format(
                                           umbHelper.GetDictionaryValue(dictionalyKey),
                                           content.GetPropertyValue<DateTime>(alias)
                                       )
                                   );
            }
            catch
            {
                return new HtmlString(string.Empty);
            }
        }
    }
}
