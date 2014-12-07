using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web;
using umbraco.NodeFactory;
using Umbraco.Core.Models;

namespace Velstand.Utilities
{
    public static class StringExtension
    {
        /// <summary>
        /// 文字列が空なら代わりの文字列を返す
        /// </summary>
        /// <param name="self">文字列</param>
        /// <param name="sub">代わりの文字列</param>
        /// <returns>string</returns>
        public static string EmptyIf(this string self, string sub)
        {
            if (string.IsNullOrEmpty(self))
            {
                return sub;
            }
            return self;
        }
    }
}