using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Core.Models;
using Examine;
using Velstand.Constants;

namespace Velstand.Models
{
    public class SearchEngineModel
    {
        private int countPerPages;
        private int currentPageNumber;
        private int contentsCount;
        private int numberOfPage;
        private UmbracoHelper umbraco;
        private HttpRequestBase request;
        private HtmlDecorator _Html { get; set; }
        public IPublishedContent CurrentPage;
        public HtmlDecorator Html { get { return _Html; } }

        public SearchEngineModel(IPublishedContent current, HttpRequestBase httpRequest, int countPerPages = 20) 
        {
            this.CurrentPage = current;
            this.countPerPages = countPerPages;
            this.request = httpRequest;
            this.umbraco = new UmbracoHelper(UmbracoContext.Current);
            this._Html = new HtmlDecorator(this);
        }

        /// <summary>
        /// 検索を実行した結果のコレクションを取得する
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPublishedContent> Contents()
        {
            IEnumerable<IPublishedContent> posts = this.CurrentPage.VHasContents();
            try
            {
                int pageNumber = 1;

                // リクエストパラメータに一致するコンテントを取得する
                foreach (var key in this.request.Params.AllKeys)
                {
                    switch (key)
                    {
                        case (VelstandRequest.PageNumber):
                            // リクエストパラメータ [page]
                            int.TryParse(this.request[key], out pageNumber);
                            break;
                        case (VelstandRequest.Date):
                            // リクエストパラメータ [date]
                            string date = this.request[key];
                            posts = this.PostsOfDate(date);
                            break;
                        case (VelstandRequest.Category):
                            // リクエストパラメータ [cat]
                            string category = this.request[key];
                            posts = posts.Where(w => Array.IndexOf(w.GetPropertyValue<string>(VelstandProperty.Category).Split(','), category) != -1);
                            break;
                        case (VelstandRequest.Tag):
                            // リクエストパラメータ [tag]
                            string tag = this.request[key];
                            posts = posts.Where(w => w.HasValue(VelstandProperty.Tag) && Array.IndexOf(w.GetPropertyValue<string>(VelstandProperty.Tag).Split(','), tag) != -1);
                            break;
                        case(VelstandRequest.Text):
                            // FIXME:2つ目以降のリクエストパラメータで渡された場合、それ以前のパラメータが無効化されてしまう
                            var searchCriteria = ExamineManager.Instance.CreateSearchCriteria();
                            var query = searchCriteria.GroupedOr(new[] { "name", "title", "introduction", "body" }, this.request[key]).Compile();
                            posts = this.umbraco.TypedSearch(query).Where(x => x.DocumentTypeAlias.StartsWith(VelstandPrefix.Content));
                            break;
                        default:
                            break;
                    }
                }

                // set page parameters
                this.currentPageNumber = pageNumber;
                this.contentsCount = posts.Count();
                this.numberOfPage = (int)Math.Ceiling((double)this.contentsCount / (double)this.countPerPages);

                // 取得件数が0以下なら全件を返す
                if (this.countPerPages <= 0)
                {
                    return posts;
                }

                pageNumber = pageNumber > 0 ? pageNumber - 1 : 0;
                return posts.Skip(pageNumber * this.countPerPages).Take(this.countPerPages);
            }
            catch
            {
                if (this.countPerPages <= 0)
                {
                    return posts;
                }
                return posts.Take(this.countPerPages);
            }
        }

        #region HtmlDecoretion
        public class HtmlDecorator
        {
            private SearchEngineModel parent;
            public HtmlDecorator(SearchEngineModel model)
            {
                this.parent = model;
            }

            /// <summary>
            /// 前のページタグを取得する
            /// </summary>
            /// <param name="wording">InnerText</param>
            /// <param name="tag">Tag Name</param>
            /// <returns></returns>
            public IHtmlString Prev(string wording, string tag = "li")
            {
                string result;
                if (this.parent.currentPageNumber > 1)
                {
                    result = "<" + tag + "><a href=\"" + this.parent.CurrentPage.VPageUrl(this.parent.currentPageNumber - 1, this.parent.request) + "\">" + wording + "</a><" + tag + ">";
                }
                else
                {
                    result = "<" + tag + " class=\"disabled\"><a href=\"#\">" + wording + "</a><" + tag + ">";
                }
                return new HtmlString(result);
            }

            /// <summary>
            /// 次のページタグを取得する
            /// </summary>
            /// <param name="wording">InnerText</param>
            /// <param name="tag">Tag Name</param>
            /// <returns></returns>
            public IHtmlString Next(string wording, string tag = "li")
            {
                string result;
                if (this.parent.currentPageNumber < this.parent.numberOfPage)
                {
                    result = "<" + tag + "><a href=\"" + this.parent.CurrentPage.VPageUrl(this.parent.currentPageNumber + 1, this.parent.request) + "\">" + wording + "</a><" + tag + ">";
                }
                else
                {
                    result = "<" + tag + " class=\"disabled\"><a href=\"#\">" + wording + "</a><" + tag + ">";
                }
                return new HtmlString(result);
            }

            /// <summary>
            /// ページネーションタグを生成する
            /// </summary>
            /// <param name="displayNum">表示する個数</param>
            /// <param name="tag">Tag Name</param>
            /// <returns></returns>
            public IHtmlString Pagination(int displayNum = 4, string tag = "li")
            {
                var result = new List<string>();
                var startNo = this.parent.currentPageNumber <= displayNum ? 1 : this.parent.currentPageNumber - displayNum;
                for (int i = startNo; i <= this.parent.numberOfPage; i++)
                {
                    var href = i == this.parent.currentPageNumber ? "#" : this.parent.CurrentPage.VPageUrl(i, this.parent.request);
                    var class_name = i == this.parent.currentPageNumber ? "active" : string.Empty;
                    result.Add(string.Format("<{0} class=\"{1}\"><a href=\"{2}\">{3}</a></{4}>", tag, class_name, href, i, tag));
                    if (i >= (startNo + displayNum + 1)) { break; }
                }
                return new HtmlString(string.Join("", result));
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// 日付でポストを絞り込む
        /// </summary>
        /// <param name="nodes">IPublishedContent</param>
        /// <param name="date">指定の日付 (yyyy or yyyyMM or yyyyMMdd)</param>
        /// <returns>IPublishedContents</returns>
        private IEnumerable<IPublishedContent> PostsOfDate(string date)
        {
            try
            {
                int dateInt;
                if (!int.TryParse(date, out dateInt))
                {
                    return this.CurrentPage.VHasContents();
                }
                switch (date.Length)
                {
                    case (4):
                        return this.CurrentPage.VHasContents().Where(w => w.GetPropertyValue<DateTime>(VelstandProperty.ReleaseDate).ToString("yyyy") == date);
                    case (6):
                        return this.CurrentPage.VHasContents().Where(w => w.GetPropertyValue<DateTime>(VelstandProperty.ReleaseDate).ToString("yyyyMM") == date);
                    case (8):
                        return this.CurrentPage.VHasContents().Where(w => w.GetPropertyValue<DateTime>(VelstandProperty.ReleaseDate).ToString("yyyyMMdd") == date);
                }
                return this.CurrentPage.VHasContents();
            }
            catch
            {
                return this.CurrentPage.VHasContents();
            }
        }
        #endregion
    } 
}
