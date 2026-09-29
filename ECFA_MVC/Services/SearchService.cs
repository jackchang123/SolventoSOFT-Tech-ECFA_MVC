namespace ECFA_MVC.Services
{
    using ECFA_MVC.Models.DTOs;
    using GufonetUtility;
    using GufonetUtility.Constants;
    using GufonetUtility.GufonetConstants;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class SearchService : ISearchService
    {
        private readonly IConfiguration _configuration;
        private readonly string _gf_host;

        private readonly IMenuUrlService _menuUrlService;

        public SearchService(IConfiguration configuration , IMenuUrlService menuUrlService)
        {
            _configuration = configuration;
            _gf_host = configuration.GetSection("GugonetSetting")["Host"] ?? string.Empty;
            _menuUrlService = menuUrlService;
        }

        public SearchResultViewModel<SearchInfo> GetSearchInfos(HashSet<string> column, GufoQueryModel model, string years = "-30")
        {
            var startQueryTime = DateTime.Now;
            var result = new SearchResultViewModel<SearchInfo>();

            string gfHost = _gf_host;
            string[] indexNames = ["GUFO_ECFA"];

            var gf = new GufonetClient(gfHost, string.Empty);
            var indexSettings = gf.GetIndexSetting().Where(w => indexNames.Contains(w.IndexName)).ToList();
            gf.SetQueryIndex(indexSettings);

            var extBuilder = new StringBuilder();
            if (model.F_word == "1") extBuilder.Append("doc;rtf;docx;odt;");
            if (model.F_ppt == "1") extBuilder.Append("ppt;pps;pptx;odp;");
            if (model.F_xls == "1") extBuilder.Append("xls;xlsx;ods;");
            if (model.F_pdf == "1") extBuilder.Append("pdf;");
            if (model.F_htm == "1") extBuilder.Append("html;htm;");
            if (model.F_zip == "1") extBuilder.Append("zip;tar;gz;");

            string tempQuery = extBuilder.ToString();
            if (!string.IsNullOrEmpty(tempQuery))
            {
                tempQuery = $"extensiondata:({tempQuery.TrimEnd(';')}) and ";
            }

            string query = string.IsNullOrEmpty(model.Keyword) ? "*" : model.Keyword;

            var rangeList = new List<string>();
            string date1 = model.Datea ?? model.Date1 ?? "";
            string date2 = model.Dateb ?? model.Date2 ?? "";

            if (!string.IsNullOrEmpty(date1)) rangeList.Add($"$date$ >= {date1}");
            if (!string.IsNullOrEmpty(date2)) rangeList.Add($"$date$ <= {date2}");

            string rangeCondition = string.Join(" and ", rangeList);

            if (!string.IsNullOrEmpty(model.FirstTitleTyperangeContent))
            {
                gf.SetSearchColumns(new List<string> { $"NodePath:{model.FirstTitleTyperangeContent}" });
            }

            gf.SetSearchModeOption(Convert.ToBoolean(int.Parse(model.S1)), Convert.ToBoolean(int.Parse(model.S2)));

            // 設定查詢模式
            gf.SetSearchMode(model.SearchMode switch
            {
                "0" => DefaultValue.SearchMode.Precise,
                "1" => DefaultValue.SearchMode.Fuzzy,
                "2" => DefaultValue.SearchMode.Topic,
                _ => DefaultValue.SearchMode.Precise
            });

            gf.SetResultOptionContentField(column);
            gf.SetQuerySentence(tempQuery + query);
            gf.SetRangeCondition(rangeCondition);

            var sortFields = new Dictionary<string, DefaultValue.SortType>
            {
                { model.Sort, DefaultValue.SortType.Desc }
            };
            gf.SetSortFields(sortFields);

            gf.Search(int.Parse(model.PageNo), int.Parse(model.Start) - 1);
            result.TotalPages = gf.GetSearchResultCount();
            result.CurrentPage = model.Start;

            // 4. 分類統計名稱轉換
            var statisticsCategorys = gf.GetFieldStatistics(DefaultValue.StatisticsType.String, "NodePath");
            List<string> uniqueNodeIds = statisticsCategorys.Keys
                .SelectMany(path => path.Split('/'))
                .Distinct()
                .OrderBy(id => id)
                .ToList();
            // 5. 【重大優化】一次性捞出所有搜尋結果對應的 Navigator，解決 N+1 查詢問題
            var searchresult = gf.GetSearchResult();
            Dictionary<string, string> dicNode = _menuUrlService.GetNodeIdDictionary();

            result.Items = searchresult.Select(d =>
            {
                var info = new SearchInfo();
                //string "tblname","ntId","ntParentId"
                string tblname = d.GetValueOrDefault("tblname") ?? "";
                string ntId = d.GetValueOrDefault("ntId") ?? "";
                string ntParentId = d.GetValueOrDefault("ntParentId") ?? "";
                info.PageUrl = _menuUrlService.GenerateUrl(tblname, ntId, ntParentId, dicNode);
                
                if (d.TryGetValue("$Title$", out var title)) info.Title = title;
                if (d.TryGetValue("$Abstract$", out var content)) info.Content = ExtendUtility.AbstractHighlight(StripHtml(content));

                if (d.TryGetValue("$Date$", out var timeStr) && DateTime.TryParse(timeStr, out var time))
                    info.Time = time;
                else
                    info.Time = DateTime.MinValue;
                return info;
            }).ToArray();

            // 6. 相關關鍵字與推薦字優化
            gf.SearchKeyword(true, true, 10, 10);

            foreach (var item in gf.GetRelatedKeyword())
            {
                if (!string.IsNullOrEmpty(item.Key) && !containsExtension(item.Key))
                {
                    result.RelatedKeywords.Add(new TermItem { Text = item.Key, Df = item.Value });
                }
            }

            foreach (var item in gf.GetSearchKeyword())
            {
                if (!string.IsNullOrEmpty(item.Key) && !containsExtension(item.Key))
                {
                    result.Keywords.Add(new TermItem { Text = item.Key, Df = item.Value });
                }
            }

            // 7. 日期統計
            DefaultValue.StatisticsType type = years switch
            {
                "-365" => DefaultValue.StatisticsType.Year,
                "-30" => DefaultValue.StatisticsType.Month,
                _ => DefaultValue.StatisticsType.Day
            };

            var statisticsColumn = gf.GetFieldStatistics(type, GufonetIndexField.DateColumn);
            foreach (var item in statisticsColumn)
            {
                string key = years switch
                {
                    "" or "-1" => item.Key,
                    "-30" => item.Key.Length >= 7 ? item.Key[..7] : item.Key, // 使用 C# 8+ 的範圍切片 [..7]
                    "-365" => item.Key.Length >= 4 ? item.Key[..4] : item.Key,
                    _ => item.Key
                };

                result.Dates.Add(new TermItem { Text = key, Df = item.Value });
            }

            result.QueryTotalSecond = (DateTime.Now - startQueryTime).TotalSeconds.ToString();
            return result;
        }
        private bool containsExtension(string keyword)
        {
            string compareString = keyword.ToLower();
            string[] extension = { "br", "doc", "docx", "html", "htm", "pdf", "xls", "xlsx", "ppt", "pps", "pptx", "rtf", "zip", "rar", "odt", "odp", "ods" };
            for (int i = 0; i < extension.Length; ++i)
            {
                if (compareString.Contains(extension[i])) return true;
            }
            return false;
        }
        private static readonly Regex CompleteTagRegex = new("<[^>]*>", RegexOptions.Compiled);
        private static readonly Regex BrokenTagRegex = new("<[^>]*$", RegexOptions.Compiled);
        public static string StripHtml(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            // 🛠️ 步驟一：先把最末端殘缺的 "<span class=" 這種東西整段蒸發掉
            string fixedInput = BrokenTagRegex.Replace(input, "");

            // 🛠️ 步驟二：再用標準 Regex 清洗掉所有完整的 HTML 標籤
            string cleanText = CompleteTagRegex.Replace(fixedInput, "");

            // 🛠️ 步驟三：移除網頁常用的特殊空白編碼
            return cleanText.Replace("&nbsp;", " ").Trim();
        }
    }
    // 原本：internal class SearchInfo
    public class SearchInfo
    {
        public int NodeId { get; set; }
        public int PageId { get; set; }
        public int PageType { get; set; }
        public string PageUrl { get; set; } = "";
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime Time { get; set; }
        public string NodePath { get; set; } = "";
    }

    // 原本：internal class SearchResultViewModel<T>
    public class SearchResultViewModel<T>
    {
        public T[] Items { get; set; } = new T[0];
        public string CurrentPage { get; set; } = "";
        public int TotalPages { get; set; }
        public List<TermItem> RelatedKeywords { get; set; } = new List<TermItem>();
        public List<TermItem> Keywords { get; set; } = new List<TermItem>();
        public List<TermItem> Dates { get; set; } = new List<TermItem>();
        public List<TermItem> Categorys { get; set; } = new List<TermItem>();
        public string QueryTotalSecond { get; set; } = "";
    }
}
