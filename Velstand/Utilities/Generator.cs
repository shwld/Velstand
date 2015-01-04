using System;

namespace Velstand.Utilities
{
    public static class Generator
    {
        /// <summary>
        /// ディレクトリを作成する
        /// </summary>
        /// <param name="path">作成したいパス</param>
        public static void CreateDirectory(string path)
        {
            var phisicalPath = System.Web.HttpContext.Current.Server.MapPath(path);
            if (!System.IO.Directory.Exists(phisicalPath))
            {
                System.IO.Directory.CreateDirectory(phisicalPath);
            }
        }

        /// <summary>
        /// ミリ秒まで含めたタイムスタンプを生成
        /// </summary>
        /// <returns>タイムスタンプ(yyyyMMddHHmmssfff)</returns>
        public static string CurrentTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}