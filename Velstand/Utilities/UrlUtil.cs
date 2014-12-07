using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.NodeFactory;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Velstand.Utilities
{
    public class UrlUtil
    {
        /// <summary>
        /// ルートURLの取得
        /// </summary>
        /// <returns>ex:http://example.com/</returns>
        public static string SchemeAndAuthority()
        {
            // ルートURL の生成
            var scheme = HttpContext.Current.Request.Url.Scheme;
            var authority = HttpContext.Current.Request.Url.Authority;
            return string.Format("{0}://{1}", scheme, authority);
        }

        /// <summary>
        /// 同名のドキュメントが存在しない場合ノードを作成する
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Node DocumentPublishOnNotExist(IContent target, Node parent)
        {
            // HACK:Linq使いたい
            Node node = null;
            foreach (Node child in parent.Children)
            {
                if (child.Name == target.Name)
                {
                    node = child;
                }
            }
            if (node != null)
            {
                return node;
            }

            // コンテンツを保存する
            var contentService = new ContentService();
            contentService.SaveAndPublishWithStatus(target, target.WriterId, false);
            return new Node(target.Id);
        }
    }
}