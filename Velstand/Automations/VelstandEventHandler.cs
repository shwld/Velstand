using System;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using Umbraco.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Events;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Core.Logging;

namespace Velstand.Automations
{
    /// <summary>
    /// イベントハンドラー
    /// </summary>
    public class VelstandEventHandler : ApplicationEventHandler
    {
        /// <summary>
        /// イベントハンドラー
        /// </summary>
        public VelstandEventHandler()
        {
            // コンテンツ保存後の処理
            ContentService.Saved += Document_Saved;

            // コンテンツ発行時の処理
            ContentService.Publishing += Document_Publishing;

            // コンテンツ発行後の処理
            ContentService.Published += Document_Published;
        }

        /// <summary>
        /// アイテム保存後に実行されるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            e.SavedEntities.ForEach(x => Automation.SavedAutoOparations(x));
        }
        
        /// <summary>
        /// アイテム発行時に実行されるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Document_Publishing(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            e.PublishedEntities.ForEach(x => Automation.PublishingAutoOparations(x));
        }

        /// <summary>
        /// アイテム発行後に実行されるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Document_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            e.PublishedEntities.ForEach(x => Automation.PublishedAutoOparations(x));

            // sitemap.xmlを生成
            SiteMapCreator.GenerateXml();
        }

    }
}