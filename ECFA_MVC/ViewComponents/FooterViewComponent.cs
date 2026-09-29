using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECFA_MVC.Models;

namespace ECFA_MVC.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly EcfaContext _context;
        public FooterViewComponent(EcfaContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // 1. 模擬從資料庫撈出來的各種原始資料
            int totalCount = await _context.LogCounts.SumAsync(x => x.LcCount);
            long rawCount = totalCount;
            string currentYear = DateTime.Now.Year.ToString();

            // 2. 實例化 Model 並把後台處理好的資料塞進去
            var model = new ECFA_MVC.ViewModels.FooterViewModel
            {
                VisitorCount = rawCount.ToString("N0"),
                LastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd")
            };

            // 3. 🌟 將整個 model 物件傳送給前端畫面
            return View("~/Views/Shared/_Footer.cshtml", model);
        }
    }
}
