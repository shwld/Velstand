using System;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Velstand.Utilities
{
    public static class MediaUtil
    {
        /// <summary>
        /// Get media folder
        /// </summary>
        /// <returns>folder</returns>
        public static IMedia GetOrGenerateHolder(string folderName = "velstand")
        {
            var mediaService = ApplicationContext.Current.Services.MediaService;
            var now = DateTime.Today;
            var parentNode = mediaService.GetRootMedia().FirstOrDefault(x => x.Name == folderName);
            IMedia folder;

            if (parentNode == null)
            {
                folder = mediaService.CreateMedia(folderName, -1, "Folder");
                parentNode = mediaService.GetOrCreate(folder, parentNode);
            }

            return parentNode;
        }

        /// <summary>
        /// 指定した親メディアの配下から同名のメディアを取得する。なければ作成して取得する
        /// </summary>
        /// <param name="service">メディアサービス</param>
        /// <param name="media">メディア</param>
        /// <param name="parent">親メディア</param>
        /// <returns>メディア</returns>
        public static IMedia GetOrCreate(this IMediaService service, IMedia media, IMedia parent)
        {
            if (parent != null)
            {
                var matchedMedia = parent.Children().FirstOrDefault(x => x.Name == media.Name);
                if (matchedMedia != null)
                    return matchedMedia;
            }

            service.Save(media);
            return media;
        }
    }
}
