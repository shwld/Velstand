using umbraco.NodeFactory;
using Umbraco.Core.Models;
using Umbraco.Core;

namespace Velstand.Utilities
{
    public class DocumentUtil
    {
        /// <summary>
        /// 同名のドキュメントが存在しない場合ノードを作成する
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Node PublishOnNotExist(IContent target, Node parent)
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
            var contentService = ApplicationContext.Current.Services.ContentService;
            contentService.SaveAndPublishWithStatus(target, target.WriterId, false);
            return new Node(target.Id);
        }
    }
}