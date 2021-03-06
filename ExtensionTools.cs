﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace LightNovelSniffer
{
    static class ExtensionTools
    {
        internal static List<string> ParseHtmlNodeToStringList(this HtmlNodeCollection nodes)
        {
            return nodes.ToList().ParseHtmlNodeToStringList();
        }

        internal static List<string> ParseHtmlNodeToStringList(this IEnumerable<HtmlNode> nodes)
        {
            return nodes
                .Select(b => b.ParseHtmlNodeToString())
                .ToList();
        }

        internal static string ParseHtmlNodeToString(this HtmlNode node)
        {
            return WebUtility.HtmlDecode(node.InnerText);
        }

        internal static bool ContainsInvarient(this string str, string recherche)
        {
            return str.ToLower().Contains(recherche.ToLower());
        }

        internal static bool ContainsType<T>(this IEnumerable<T> collection, Type type)
        {
            return collection.Any<T>(i => i.GetType() == type);
        }
    }
}