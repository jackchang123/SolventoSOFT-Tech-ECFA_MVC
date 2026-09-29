using Microsoft.AspNetCore.Mvc;

namespace ECFA_MVC.ViewComponents // 記得換成你的專案名稱空間
{
    public class SocialMediaListViewComponent : ViewComponent
    {
        // InvokeAsync 是固定名稱，.NET Core 會自動呼叫它
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SocialMediaViewModel();

            // 1. 取得目前的 Route 資料（Controller 與 Action 名稱）
            var routeData = Request.RouteValues;
            string controller = routeData["controller"]?.ToString()?.ToLower() ?? "";
            string action = routeData["action"]?.ToString()?.ToLower() ?? "";

            // 2. 根據不同的網址（Controller），動態載入不同數量的項目
            if (controller == "news")
            {
                model.ShowRss = true;
                model.ShowFb = true;
            }
            else if (controller == "article" && action == "detail")
            {
                model.ShowFb = true;
                model.ShowX = true;
                model.ShowPrint = true;
            }
            else
            {
                // 其他頁面的預設狀態
                model.ShowFb = true;
                model.ShowX = true;
            }

            // 3. 自動抓取目前的完整網址
            model.CurrentUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            // 4. 自動抓取目前的網頁標題
            // 實務上我們會在 Controller Action 寫 ViewData["Title"] = "文章標題"，這裡可以直接共享讀取
            model.PageTitle = ViewContext.ViewData["Title"]?.ToString() ?? "預設網站名稱";

            return View(model);
        }
    }

    // 用來儲存開關狀態的簡單 Model
    public class SocialMediaViewModel
    {
        public bool ShowRss { get; set; } = false;
        public bool ShowFb { get; set; } = false;
        public bool ShowX { get; set; } = false;

        public bool ShowPlurk { get; set; } = false;

        public bool ShowPrint { get; set; } = false;

        // 分享所需的資料
        public string CurrentUrl { get; set; } = string.Empty;
        public string PageTitle { get; set; } = string.Empty;
    }
}