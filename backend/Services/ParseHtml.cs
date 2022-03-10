using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace webbot.Services
{

    public interface IParseHtml
    {
        string InnerText(string html, string xpath);
        HtmlNodeCollection Nodes(string html, string xpath);
        public string Regexp(string html, string xpath, string regexpString);
    }

    public class ParseHtml : IParseHtml
    {
        public string InnerText(string html, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var result = doc.DocumentNode.SelectSingleNode(xpath);

            return result?.InnerText.Replace("\t", "").Replace("\n", "").Replace("\r", "").Trim(' ') ?? "parseHtmlNULL";
        }

        public HtmlNodeCollection Nodes(string html, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.SelectNodes(xpath);
        }

        public string Regexp(string html, string xpath, string regexpString)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var section = doc.DocumentNode.SelectSingleNode(xpath);

            var regex = new Regex(regexpString);
          
            return regex.Match(section.InnerHtml).Value;
        }
    }
}
