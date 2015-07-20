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
using Velstand.Models;
using Velstand.Utilities;

namespace Velstand.Automations
{
    public class BlogCreator
    {
        private ContentService contentService = new ContentService();
        private UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
        private DateTime releaseDate;
        private IContent node;
        private string folderNameSetting = string.Empty;
        private IPublishedContent holderTop;

        public BlogCreator(IContent content)
        {
            this.node = content;
            this.releaseDate = ContentCreator.ReleaseDate(content);

            var parent = this.umbracoHelper.TypedContent(this.node.ParentId); 
            this.holderTop = parent != null ? parent.VHolderTop() : null;
            if (holderTop != null)
            {
                this.folderNameSetting = holderTop.GetPropertyValue<string>(VelstandProperty.AutoMakeHolderName);
            }
        }

        /// <summary>
        /// ブログフォルダを自動作成する
        /// </summary>
        public void CreateHolder()
        {
            if (!this.IsValid()) { return; }

            Node parentNode = new Node(this.holderTop.Id);

            // 年フォルダ、月フォルダの作成
            if (this.folderNameSetting == VelstandConstant.BlogHolderNameYM1 ||
                this.folderNameSetting == VelstandConstant.BlogHolderNameYM2 ||
                this.folderNameSetting == VelstandConstant.BlogHolderNameYMD1 ||
                this.folderNameSetting == VelstandConstant.BlogHolderNameYMD2 )
            {
                parentNode = DocumentUtil.PublishOnNotExist(
                                            BlogHolder(this.releaseDate.Year.ToString(), parentNode.Id, this.node.WriterId), 
                                            parentNode);

                string mm = "MMMM";
                if (this.folderNameSetting == VelstandConstant.BlogHolderNameYM2 || 
                    this.folderNameSetting == VelstandConstant.BlogHolderNameYMD2)
                {
                    mm = "MM";
                }
                parentNode = DocumentUtil.PublishOnNotExist(
                                            BlogHolder(this.releaseDate.ToString(mm), parentNode.Id, this.node.WriterId), 
                                            parentNode);
                this.node.ParentId = parentNode.Id;
            }

            // 日フォルダを作成する
            if (this.folderNameSetting == VelstandConstant.BlogHolderNameYMD1 ||
                this.folderNameSetting == VelstandConstant.BlogHolderNameYMD2)
            {
                parentNode = DocumentUtil.PublishOnNotExist(
                                            BlogHolder(this.releaseDate.Day.ToString("00"), parentNode.Id, this.node.WriterId), 
                                            parentNode);
                this.node.ParentId = parentNode.Id;
            }

            // 年月フォルダを作成する
            if (this.folderNameSetting == VelstandConstant.BlogHolderNameYM3)
            {
                parentNode = DocumentUtil.PublishOnNotExist(
                                            BlogHolder(this.releaseDate.ToString("yyyyMM"), parentNode.Id, this.node.WriterId), 
                                            parentNode);
                this.node.ParentId = parentNode.Id;
            }

            return;
        }

        public void SaveImages()
        {
            if (this.node.HasProperty(VelstandProperty.Body))
            {
                string body = this.node.GetValue<string>(VelstandProperty.Body);
                this.node.SetValue(VelstandProperty.Body, ContentCreator.ConvertBase64ImageAndGet(body));
            }
        }

        public void SaveOrPublish()
        {
            // ドキュメントを公開する
            if (this.node.Published)
            {
                contentService.SaveAndPublishWithStatus(this.node, this.node.WriterId, false);
            }
            else
            {
                contentService.Save(this.node, this.node.WriterId, false);
            }
        }

        /// <summary>
        /// BlogHolder
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IContent BlogHolder(string name, int parentId, int userId)
        {
            var contentService = new ContentService();
            var target = contentService.CreateContent(
                    name,
                    parentId, 
                    VelstandDocumentType.BlogHolder,
                    userId
                );

            try
            {
                // パンくず非表示フラグをtrueにする
                target.SetValue(VelstandProperty.IsHiddenBreadCrumb, true);
            }
            catch(Exception ex)
            {
                LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, ex.Message, ex);
            }

            return target;
        }

        private Boolean IsValid()
        {
            if (this.node.ContentType.Alias != VelstandDocumentType.BlogPost ||
                this.holderTop == null || this.umbracoHelper.TypedContent(this.node.Id) != null)
            {
                return false;
            }
            return true;
        }
    }
}