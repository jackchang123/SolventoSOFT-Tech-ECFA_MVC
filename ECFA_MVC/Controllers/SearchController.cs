using ECFA_MVC.Models;
using ECFA_MVC.Models.DTOs;
using ECFA_MVC.Services;
using GufonetUtility;
using GufonetUtility.Constants;
using GufonetUtility.GufonetConstants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Text.Json;

namespace ECFA_MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISearchService _searchService;
        private readonly ILogger<SearchController> _logger;
        private readonly EcfaContext _context;

        HashSet<string> column = new HashSet<string>
                {
            GufonetIndexField.NameColumn,
                    GufonetIndexField.TitleColumn,
                    GufonetIndexField.AbstractColumn,
                    GufonetIndexField.ContentColumn,
                    GufonetIndexField.DateColumn,
                    GufonetIndexField.IndexIdColumn,
                    "tblname","ntId","ntParentId"
                };

        public SearchController(ILogger<SearchController> logger, EcfaContext context, IConfiguration configuration, ISearchService searchService)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _searchService = searchService;
        }
        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            PropertyNamingPolicy = null
        };

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetSearchResult([FromForm] GufoQueryModel model)
        {
            // .NET 10 建議使用更現代化的 IActionResult 作為回傳型態
            var jsonFormat = new JsonFormat();
            try
            {
                // 註：如果你原本的 DataTable workTable 沒有用到，建議移除以提升效能
                model.Keyword = replaceQueryStr2(model.Keyword);

                SearchResultViewModel<SearchInfo> result = _searchService.GetSearchInfos(column, model);

                jsonFormat.Error = false;

                jsonFormat.Data = result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋時發生未知異常");

                jsonFormat.Error = true;

                #if DEBUG
                    // 開發環境：可以保留詳細訊息方便 Debug
                    jsonFormat.Message = ex.Message;
                #else
                    // 生產環境（正式上線）：回傳模糊化的通用提示，徹底杜絕資安風險
                    jsonFormat.Message = "系統繁忙或發生未知錯誤，請稍後再試。"; 
                #endif
            }

            return Json(jsonFormat, DefaultJsonOptions);
        }

        public string replaceQueryStr2(string AStr)
        {
            string sStr = AStr.Trim();

            if ((sStr.IndexOf(" ") < 0) && (sStr.IndexOf("\"") < 0))
                return sStr;

            List<string[]> lstStr = new List<string[]>();

            string sTemp = "";
            bool bFindDD = false;
            for (int i = 0; i < sStr.Length; i++)
            {
                if (sStr[i] == '"')
                {
                    if (bFindDD)
                    {
                        if (sTemp != "")
                        {
                            lstStr.Add(new string[] { sTemp, "D" });
                            sTemp = "";
                        }
                        bFindDD = false;
                    }
                    else
                    {
                        if (sTemp != "")
                        {
                            lstStr.Add(new string[] { sTemp, "" });
                            sTemp = "";
                        }
                        bFindDD = true;
                    }

                    continue;
                }

                sTemp += sStr[i];
            }

            if (sTemp != "")
            {
                if (bFindDD)
                    lstStr.Add(new string[] { sTemp, "D" });
                else
                    lstStr.Add(new string[] { sTemp, "" });
            }

            string sResult = "";
            foreach (string[] item in lstStr)
            {
                string sItemData = item[0];

                if (item[1] == "D")
                    sResult += (((sResult == "") ? "" : " ") + ("\"" + sItemData + "\""));
                else
                {
                    string sTempData = sItemData;
                    sTempData = sTempData.Replace(" OR ", "_$$$_").Replace(" or ", "_$$$_").Replace(" Or ", "_$$$_").Replace(" | ", "_$$$_");
                    sTempData = sTempData.Replace(" AND ", " ").Replace(" and ", " ").Replace(" And ", " ");
                    sTempData = sTempData.Replace(" ", " AND ");
                    sTempData = sTempData.Replace("_$$$_", " OR ");

                    sResult += (((sResult == "") ? "" : ((sTempData[0] != ' ') ? "" : "")) + sTempData);
                }
            }

            return sResult;
        }
    }
}
