namespace ECFA_MVC.Services
{
    using ECFA_MVC.Models;
    using ECFA_MVC.Models.DTOs;
    using GufonetUtility;
    using GufonetUtility.Constants;
    using GufonetUtility.GufonetConstants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MenuUrlService : IMenuUrlService
    {
        private readonly EcfaContext _context;

        public MenuUrlService(EcfaContext context)
        {
            _context = context;
        }
        public Dictionary<string, string> GetNodeIdDictionary()
        {
            var menuData = _context.Ecfamenus
                .Where(m => !m.Block
                         && m.PrgPath != null
                         && m.PrgPath != ""
                         && m.NodeType == "9"
                         && !m.PrgPath.Contains("?c=")
                         && !m.PrgPath.Contains("&c="))
                .Select(m => new { m.PrgPath, m.NodeId })
                .AsNoTracking()
                .ToList();

            return menuData
                .Select(m => new
                {
                    // 分割問號並取出主檔名
                    Key = m.PrgPath?.Split('?')[0] ?? "",
                    Value = m.NodeId.ToString()
                })
                .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.First().Value,
                    StringComparer.OrdinalIgnoreCase // 💡 關鍵：忽略大小寫
                );
        }

        /// <summary>
        /// 網址生成邏輯（已優化為 .NET 10 Switch 表達式）
        /// </summary>
        public string GenerateUrl(string tblName, string ntId, string ntParentId, Dictionary<string, string> dicNode)
        {
            // 💡 區域函式：因為字典已忽略大小寫，不需再呼叫 .ToUpper()
            string GetNidParam(string page, bool appendAmpersand = false)
            {
                if (dicNode.TryGetValue(page, out var nid))
                {
                    return appendAmpersand ? $"nid={nid}&" : $"?nid={nid}";
                }
                return "";
            }

            return tblName switch
            {
                "Action" => $"ShowAction.aspx?{GetNidParam("ActionList.aspx", true)}id={ntId}",
                "News" => $"ShowNews.aspx?{GetNidParam("NewsList.aspx", true)}id={ntId}",
                "FAQ" => $"ShowFAQ.aspx?{GetNidParam("FAQs.aspx", true)}id={ntId}",
                "ATSFAQ" => $"ShowATSFAQ.aspx?{GetNidParam("ATSFAQList.aspx", true)}id={ntId}", // 🐛 已修正原版 Bug

                "DMAd" => $"DmadList.aspx{GetNidParam("DmadList.aspx")}",
                "ECFADoc" => $"DownloadDoc.aspx{GetNidParam("DownloadDoc.aspx")}",
                "Event" => $"Event.aspx{GetNidParam("Event.aspx")}",

                "Service" => ntParentId == "12"
                    ? $"ServiceCertiDoc.aspx{GetNidParam("ServiceCertiDoc.aspx")}"
                    : $"EcfaCertiDoc.aspx{GetNidParam("EcfaCertiDoc.aspx")}",

                "Pages" => $"ShowDetail.aspx?nid={ntParentId}&pid={ntId}",
                _ => "#"
            };
        }
    }
}