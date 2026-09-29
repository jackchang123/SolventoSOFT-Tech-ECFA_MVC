using ECFA_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ECFA_MVC.ViewComponents
{
    public class NewsListViewComponent : ViewComponent
    {
        private readonly EcfaContext _context;
        public NewsListViewComponent(EcfaContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int count = 5)
        {
            // 從資料庫抓取最新的 5 則公告
            var newsList = await _context.News
                                         .OrderByDescending(x => x.NtPubDate).OrderByDescending(x => x.NtId)
                                         .Take(count)
                                         .ToListAsync();

            // 回傳局部視圖，並把資料傳過去
            return View(newsList);
        }
    }
}
