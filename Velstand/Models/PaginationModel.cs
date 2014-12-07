using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Umbraco.Web;
using Umbraco.Core.Models;

namespace Velstand.Models
{
    public class PaginationModel
    {
        private IPublishedContent content;
        private IEnumerable<IPublishedContent> contents;
        private HtmlDecorator _Html { get; set; }
        public HtmlDecorator Html { get { return _Html; } }

        public PaginationModel(IPublishedContent content) :this(
            content, 
            (new SearchEngineModel(content.VHolderTop(), httpRequest: null, countPerPages: -1)).Contents()
        ) {}
        public PaginationModel(IPublishedContent content, IEnumerable<IPublishedContent> contents)
        {
            this.content = content;
            this.contents = contents;
            this._Html = new HtmlDecorator(this);
        }

        #region HtmlDecoretion
        public class HtmlDecorator
        {
            private PaginationModel parent;
            public HtmlDecorator(PaginationModel model)
            {
                this.parent = model;
            }

            public IHtmlString Next(string format = "{0}", string alias = "title")
            {
                var next = this.parent.content.VGetNext(this.parent.contents);
                if (next == null) { return new HtmlString(string.Empty); }
                return new HtmlString(
                                        string.Format(
                                            format, 
                                            string.Format("<a href=\"{0}\">{1}</a>", next.Url, next.GetPropertyValue<string>(alias))
                                        )
                                    );
            }

            public IHtmlString Previous(string format = "{0}", string alias = "title")
            {
                var previous = this.parent.content.VGetPrevious(this.parent.contents);
                if (previous == null) { return new HtmlString(string.Empty); }
                return new HtmlString(
                                        string.Format(
                                            format, 
                                            string.Format("<a href=\"{0}\">{1}</a>", previous.Url, previous.GetPropertyValue<string>(alias))
                                        )
                                    );
            }
        }
        #endregion
    }
}
