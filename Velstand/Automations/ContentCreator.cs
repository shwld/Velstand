using System;
using System.Web;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Velstand.Constants;
using HtmlAgilityPack;
using System.IO;
using Velstand.Utilities;


namespace Velstand.Automations
{
    public static class ContentCreator
    {
        /// <summary>
        /// 未公開コンテントの公開日
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static DateTime ReleaseDate(IContent content)
        {
            var releaseDate = (DateTime)(content.ReleaseDate ?? content.CreateDate);
            if (releaseDate == new DateTime())
            {
                releaseDate = DateTime.TryParse(content.GetValue<string>(VelstandProperty.ReleaseDate), out releaseDate) ? releaseDate : DateTime.Now;
            }
            return releaseDate;
        }

        public static string ConvertBase64ImageAndGet(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) { return text; }
            var mediaService = ApplicationContext.Current.Services.MediaService;
            IMedia folder = MediaUtil.GetOrGenerateHolder();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(text);
            foreach (var img in htmlDoc.DocumentNode.SelectNodes("//img"))
            {
                string[] src = img.GetAttributeValue("src", "").Split(',');
                if (src.Count() != 2 && !src[0].Trim().ToLower().EndsWith("base64"))
                {
                    continue;
                }
                byte[] imageBytes = System.Convert.FromBase64String(src[1].Trim());
                using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                    try
                    {
                        ms.Write(imageBytes, 0, imageBytes.Length);
                        IMedia media = mediaService.CreateMedia(img.GetAttributeValue("alt", ""), folder.Id, "Image");
                        media.SetValue("umbracoFile", media.Name, ms);
                        mediaService.Save(media);
                        img.SetAttributeValue("src", media.Path);
                    }
                    catch(Exception)
                    {
                    }
                }
            }

            return htmlDoc.ToString();
        }

    }
}
