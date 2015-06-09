using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using umbraco.BusinessLogic;
using Umbraco.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Events;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Core.Logging;
using Velstand.Constants;

namespace Velstand.Models
{
    public static class IPublishedContentExtension
    {
        /// <summary>
        /// コンテンツを取得する
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> VHasContents(this IPublishedContent content)
        {
            try
            {
                return content.DescendantsOrSelf()
                                     .Where(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Content))
                                     .OrderByDescending(y => y.GetPropertyValue<DateTime>(VelstandProperty.ReleaseDate));
            }
            catch
            {
                return new List<IPublishedContent>();
            }
        }

        /// <summary>
        /// ホルダーを取得する
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IPublishedContent VHolder(this IPublishedContent content)
        {
            if (content.DocumentTypeAlias.StartsWith(VelstandPrefix.Root))
            {
                return content.DescendantsOrSelf().FirstOrDefault(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Holder));
            }
            else
            {
                return content.AncestorsOrSelf().FirstOrDefault(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Holder));
            }
        }

        /// <summary>
        /// ルートを取得する
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IPublishedContent VRoot(this IPublishedContent content)
        {
            return content.AncestorsOrSelf().LastOrDefault(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Root));
        }

        /// <summary>
        /// ホルダー(TOP)を取得する。
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IPublishedContent VHolderTop(this IPublishedContent content)
        {
            if (content.DocumentTypeAlias.StartsWith(VelstandPrefix.Root))
            {
                return content.DescendantsOrSelf().FirstOrDefault(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Holder));
            }
            else
            {
                return content.AncestorsOrSelf().LastOrDefault(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Holder));
            }
        }

        /// <summary>
        /// ブログホルダー(TOP)なければルートを取得する
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IPublishedContent VBlogHolderTopOrRoot(this IPublishedContent content)
        {
            if (content.DocumentTypeAlias.EndsWith(VelstandSuffix.Blog) ||
                content.DocumentTypeAlias == VelstandDocumentType.BlogHolderTop)
            {
                return content.VHolderTop();
            }
            else
            {
                return content.VRoot();
            }
        }

        /// <summary>
        /// コンテンツの中から次の１件を取得する
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static IPublishedContent VGetNext(this IPublishedContent content, IEnumerable<IPublishedContent> contents)
        {
            try
            {
                return contents.TakeWhile(x => x.Id != content.Id).Last(); ;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// コンテンツの中から前の１件を取得する
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static IPublishedContent VGetPrevious(this IPublishedContent content, IEnumerable<IPublishedContent> contents)
        {
            try
            {
                return contents.SkipWhile(x => x.Id != content.Id).Skip(1).First(); ;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// パンくずリストを取得する
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> VBreadCrumbs(this IPublishedContent current)
        {
            var breadcrumbs = new List<IPublishedContent>();
            while (current.Parent != null && current != null)
            {
                current = current.Parent;
                if ( !current.GetPropertyValue<Boolean>(VelstandProperty.IsHiddenBreadCrumb) )
                {
                    breadcrumbs.Insert(0, current);
                }
            }
            return breadcrumbs;
        }

        /// <summary>
        /// カテゴリーの一覧を取得する
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> VCategories(this IPublishedContent current)
        {
            return current.VRoot().Descendants().Where(w => w.DocumentTypeAlias.StartsWith(VelstandPrefix.Category));
        }

        /// <summary>
        /// MultiNodeTreePickerで選択されたノードを取得する
        /// </summary>
        /// <param name="propertyName">MultiNodePickerのプロパティエイリアス</param>
        /// <returns>IPublishedContents(List)</returns>
        public static IEnumerable<IPublishedContent> VMultiNodes(this IPublishedContent content, string propertyAlias)
        {
            try
            {
                var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
                var contents = content.GetPropertyValue<string>(propertyAlias).Split(',').Select(s => int.Parse(s)).ToList<int>();

                return contents.Select(x => umbracoHelper.TypedContent(x)).Where(y => y != null);
            }
            catch
            {
                return new List<IPublishedContent>();
            }
        }

        /// <summary>
        /// カテゴリー検索結果一覧ページのURLを取得
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static string VCategoryUrl(this IPublishedContent content, int catId, HttpRequestBase request = null)
        {
            return string.Format("{0}?{1}", content.Url, SubstitutedQueryString(request, VelstandRequest.Category, catId.ToString() ) );
        }

        /// <summary>
        /// タグ検索結果一覧ページのURLを取得
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static string VTagUrl(this IPublishedContent content, string tagName, HttpRequestBase request = null)
        {
            return string.Format("{0}?{1}", content.Url, SubstitutedQueryString(request, VelstandRequest.Tag, tagName));
        }

        /// <summary>
        /// 日付検索結果一覧ページのURLを取得
        /// </summary>
        /// <param name="catId"></param>
        /// <returns></returns>
        public static string VDateUrl(this IPublishedContent content, string date, HttpRequestBase request = null)
        {
            return string.Format("{0}?{1}", content.Url, SubstitutedQueryString(request, VelstandRequest.Date, date ) );
        }

        /// <summary>
        /// ページ番号を指定したURLを取得
        /// </summary>
        /// <param name="pageNumber">Page Number</param>
        /// <returns>Url</returns>
        public static string VPageUrl(this IPublishedContent content, int pageNumber, HttpRequestBase request = null)
        {
            return string.Format("{0}?{1}", content.Url, SubstitutedQueryString(request, VelstandRequest.PageNumber, pageNumber.ToString() ) );
        }

        private static string SubstitutedQueryString(HttpRequestBase baseRequest, string key, string value)
        {
            var result = string.Format("{0}={1}", key, value);
            if (baseRequest != null)
            {
                var requestParams = baseRequest.Params.AllKeys.ToList().Where(x => Array.IndexOf(VelstandRequest.Properties(), x) != -1 ).Select(x => x + "=" + (x == key ? value : baseRequest[x])).ToList();
                if (!baseRequest.Params.AllKeys.Contains(key))
                {
                    requestParams.Add(result);
                }
                result = string.Join("&", requestParams);
            }
            return result;
        }
    }
}