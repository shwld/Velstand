using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace Velstand.Constants
{
    /// <summary>
    /// 接頭辞
    /// </summary>
    public static class VelstandPrefix
    {
        /// <summary>
        /// コンテンツ
        /// </summary>
        public const string Content = "VelstandContent";

        /// <summary>
        /// ホルダー
        /// </summary>
        public const string Holder = "VelstandHolder";

        /// <summary>
        /// ルート
        /// </summary>
        public const string Root = "VelstandRoot";
        
        /// <summary>
        /// 機能
        /// </summary>
        public const string Function = "VelstandFunction";

        /// <summary>
        /// カテゴリー
        /// </summary>
        public const string Category = "VelstandPrivateCategory";

        /// <summary>
        /// プライベートアイテム
        /// </summary>
        public const string PrivateItem = "VelstandPrivate";

        /// <summary>
        /// エラーページ
        /// </summary>
        public const string Error = "VelstandInfomationError";
    }

    /// <summary>
    /// 接尾語
    /// </summary>
    public static class VelstandSuffix
    {
        public const string Blog = "Blog";
    }

    /// <summary>
    /// ドキュメントタイプ
    /// </summary>
    public static class VelstandDocumentType
    {
        /// <summary>
        /// ブログ投稿
        /// </summary>
        public const string BlogPost = "VelstandContentBlog";

        /// <summary>
        /// ブログランディング
        /// </summary>
        public const string BlogHolderTop = "VelstandHolderBlogtop";

        /// <summary>
        /// ブログホルダー
        /// </summary>
        public const string BlogHolder = "VelstandHolderBlog";

        /// <summary>
        /// ブログルート
        /// </summary>
        public const string BlogRoot = "VelstandRoot";

        /// <summary>
        /// ブログセッティング
        /// </summary>
        public const string Setting = "VelstandPrivateSettings";

        /// <summary>
        /// カテゴリーホルダー
        /// </summary>
        public const string CategoryHolder = "VelstandPrivateholderCategory";

        /// <summary>
        /// カテゴリー
        /// </summary>
        public const string Category = "VelstandPrivateCategory";
    }

    /// <summary>
    /// プロパティ
    /// </summary>
    public static class VelstandProperty
    {
        /// <summary>
        /// 公開日付
        /// </summary>
        public const string ReleaseDate = "releaseDate";

        /// <summary>
        /// 自動作成されるフォルダの設定
        /// </summary>
        public const string AutoMakeHolderName = "autoMakeHolderName";

        /// <summary>
        /// カテゴリー
        /// </summary>
        public const string Category = "category";

        /// <summary>
        /// タグ
        /// </summary>
        public const string Tag = "tag";

        /// <summary>
        /// パンくずリストに表示しないフラグ
        /// </summary>
        public const string IsHiddenBreadCrumb = "pankuzuFlg";

        /// <summary>
        /// タイトル
        /// </summary>
        public const string Title = "title";
    }
    
    /// <summary>
    /// 定数
    /// </summary>
    public static class VelstandConstant
    {
        /// <summary>
        /// /yyyy/mmmm/dd
        /// </summary>
        public const string BlogHolderNameYMD1 = "/yyyy/mmmm/dd";

        /// <summary>
        /// /yyyy/mm/dd
        /// </summary>
        public const string BlogHolderNameYMD2 = "/yyyy/mm/dd";

        /// <summary>
        /// /yyyy/mmmm
        /// </summary>
        public const string BlogHolderNameYM1 = "/yyyy/mmmm";

        /// <summary>
        /// /yyyy/mm
        /// </summary>
        public const string BlogHolderNameYM2 = "/yyyy/mm";

        /// <summary>
        /// /yyyymm
        /// </summary>
        public const string BlogHolderNameYM3 = "/yyyymm";
    }

    /// <summary>
    /// リクエストパラメータ
    /// </summary>
    public static class VelstandRequest
    {
        /// <summary>
        /// ページング時のページ番号
        /// </summary>
        public const string PageNumber = "page";

        /// <summary>
        /// 日付による絞り込み
        /// </summary>
        public const string Date = "date";

        /// <summary>
        /// カテゴリーによる絞り込み
        /// </summary>
        public const string Category = "cat";

        /// <summary>
        /// タグによる絞り込み
        /// </summary>
        public const string Tag = "tag";

        /// <summary>
        /// テキストサーチ
        /// </summary>
        public const string Text = "text";

        public static string[] Properties() {
            return new string[] {"page", "date", "cat", "text"};
        }
    }
}