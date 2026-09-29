using ECFA_MVC.Models;

namespace ECFA_MVC.Helpers
{
    public static class MenuUrlHelper
    {
        public static string GetNodeUrl(Ecfamenu item)
        {
            // 安全檢查，預防資料庫欄位為空
            string path = item.PrgPath?.Trim() ?? string.Empty;
            string pageId = item.PageId.ToString();

            // 1. 處理外站連結 (包含 ??)
            if (path.Contains("??"))
            {
                return path.Replace("??", "");
            }

            // 如果路徑本來就是空的，直接返回破折號或當前頁
            if (string.IsNullOrEmpty(path))
            {
                return "#";
            }

            if (!path.StartsWith("/") && !path.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                path = "/" + path;
            }

            // 2. 處理內部連結參數
            // 判斷 pageId 是否有效（非 "0" 且非空字串）
            string pidParam = (pageId == "0" || string.IsNullOrEmpty(pageId))
                ? ""
                : $"&pid={pageId}";

            // 判斷原本路徑是否已有帶其他參數 (?)，決定用 & 還是 ? 串接 nid
            string separator = path.Contains("?") ? "&" : "?";

            // 3. 組合最終網址
            return $"{path}{separator}nid={item.NodeId}{pidParam}";
        }
    }
}
