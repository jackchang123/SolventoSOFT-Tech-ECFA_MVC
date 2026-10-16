using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class ATSFAQController : Controller
    {
        private readonly EcfaContext _context;

        public record DropdownItem(int Value, string Text);

        public ATSFAQController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("categories/{ntParentId:int}")]
        public async Task<IActionResult> GetCategories(int ntParentId)
        {
            // 如果面向選「全部(-1)」，類別就沒有內容
            if (ntParentId == -1) return Ok(Array.Empty<DropdownItem>());

            var categories = await _context.Atsfaqs
                .Where(f => f.NtParentId == ntParentId)
                .Select(f => new DropdownItem(f.NtId, f.NtTitle))
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetATSFAQList(string categoryId1, string categoryId2, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Atsfaqs.AsQueryable();

            // 【核心優化 1】在記憶體中先把字串轉成 int，絕對不要在 LINQ 條件式裡面做 ToString()
            int.TryParse(categoryId1, out int cat1);
            int.TryParse(categoryId2, out int cat2);

            if (cat1 == -1)
            {
                query = query.Where(f => f.NtParentId != -1);
            }
            else if (cat2 == -1)
            {
                // 【核心優化 2】移除 await 與 ToListAsync()，改成純 IQueryable 子查詢
                // 這時候完全不觸發資料庫，只是定義 SQL 的敘述架構
                var childCategoryIds = _context.Atsfaqs
                    .Where(sub => sub.NtParentId == cat1)
                    .Select(sub => sub.NtId);

                // 這裡的 Contains 會被 EF 完美翻譯成 SQL 的 WHERE NtParentId IN (SELECT NtId FROM ...)
                query = query.Where(f => childCategoryIds.Contains(f.NtParentId));
            }
            else
            {
                // 使用轉型後的 int 進行比較，資料庫才能吃到 Index (索引)
                query = query.Where(f => f.NtParentId == cat2);
            }

            query = query.OrderByDescending(f => f.NtModDate);

            // 直到這一步，EF 才會把優化後的單一條 SQL 送往資料庫，速度會差好幾百倍
            var pagedData = await PaginatedList<Atsfaq>.CreateAsync(query, pageNumber, pageSize);

            return Json(new { totalCount = pagedData.TotalCount, items = pagedData.Items });
        }
    }
}
