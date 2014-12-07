using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;
using Umbraco.Core.Models;
using Velstand.Models;
using Velstand.Constants;

namespace Velstand.Models
{
    public static class IPublishedContentCollectionExtension
    {
        /// <summary>
        /// カテゴリーに一致するコンテンツを取得する
        /// （検索したいカテゴリーIDをカンマ区切りの文字列で入力し、一致するカテゴリーを多く保有するコンテントを優先して取得する）
        /// </summary>
        /// <param name="contents">検索元</param>
        /// <param name="categoryIds">検索するカテゴリーのID(ex:"1234,1235,1236")</param>
        /// <param name="id">検索から除外したいコンテントのID</param>
        /// <returns>blogFlexyContents</returns>
        public static IEnumerable<IPublishedContent> VWhereCategory(this IEnumerable<IPublishedContent> contents, string categoryIds, int id = 0)
        {
            try
            {
                var result_contents = new List<SortableModel>();

                // コンテンツを再帰的に検索し、検索対象のカテゴリー配列と一致するカテゴリーの保有数を[Position]へセットする
                foreach ( var sortModel in contents.Select(x => new SortableModel(x)) )
                {
                    var cancated = categoryIds.Split(',')
                                            .Concat(sortModel.Content.GetPropertyValue<string>("category")
                                            .Split(','));
                    sortModel.PrimarySortOrder = cancated.Count() - cancated.Distinct().Count();
                    result_contents.Add(sortModel);
                }

                return result_contents.Where(x => x.PrimarySortOrder != 0 && x.Content.Id != id)
                                                .OrderByDescending(y => y.Content.GetPropertyValue<DateTime>(VelstandProperty.ReleaseDate))
                                                .OrderByDescending(z => z.PrimarySortOrder)
                                                .Select(xx => xx.Content);
            }
            catch
            {
                return new List<IPublishedContent>();
            }
        }
    }
}
