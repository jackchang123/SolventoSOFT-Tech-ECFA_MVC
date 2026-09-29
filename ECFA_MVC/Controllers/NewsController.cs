using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class NewsController : Controller
    {
        private readonly EcfaContext _context;
        public NewsController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetNewsList(string nid, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.News.AsQueryable();
            var twelveMonthsAgo = DateTime.Now.AddMonths(-12); // 在 C# 算好，例如 2025-08-25
            if (nid == "1139")
                query = query.Where(f => f.NtPubDate >= twelveMonthsAgo);
            else if(nid == "1140")
                query = query.Where(f => f.NtPubDate < twelveMonthsAgo);
            else
                query = query.Where(f => f.NtPubDate < twelveMonthsAgo);
            query = query.OrderByDescending(f => f.NtPubDate);

            var pagedData = await PaginatedList<News>.CreateAsync(query, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }
        [HttpGet]
        public async Task<IActionResult> ShowNew(int nid, int pid)
        {
            // 1. 讀取 Pages 資料表中的主文章
            var pageData = await _context.News
                .FirstOrDefaultAsync(p => p.NtId == pid);

            if (pageData == null)
            {
                return NotFound(); // 若找不到網頁，回傳 404
            }

            // 3. 組裝成前端專用的 PageDetailViewModel
            var viewModel = new PageDetailViewModel
            {
                Title = pageData.NtTitle,     // 假設 Pages 表中的標題欄位叫 ntTitle
                Content = pageData.NtContent, // 假設 Pages 表中的內文欄位叫 ntContent
            };

            return View(viewModel);
        }
    }
}
