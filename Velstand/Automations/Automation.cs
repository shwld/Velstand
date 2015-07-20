using System;
using System.Linq;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Events;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Core.Logging;
using Velstand.Constants;

namespace Velstand.Automations
{
    /// <summary>
    /// 自動化クラス
    /// </summary>
    public static class Automation
    {
        /// <summary>
        /// 自動オペレーションを実行するメイン処理
        /// </summary>
        /// <param name="node"></param>
        public static void SavedAutoOparations(IContent node){
            // ブログのフォルダを自動作成する
            var blogCreator = new BlogCreator(node);
            blogCreator.CreateHolder();
            blogCreator.SaveImages();
            blogCreator.SaveOrPublish();
        }

        /// <summary>
        /// 自動オペレーションを実行するメイン処理
        /// </summary>
        /// <param name="node"></param>
        public static void PublishingAutoOparations(IContent node)
        {
            node.SetValue(VelstandProperty.ReleaseDate, ContentCreator.ReleaseDate(node).ToString());
        }

        /// <summary>
        /// 自動オペレーションを実行するメイン処理
        /// </summary>
        /// <param name="node"></param>
        public static void PublishedAutoOparations(IContent node)
        {
        }
    }
}