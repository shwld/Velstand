using System;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Velstand.Constants;

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
    }
}
