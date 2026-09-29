using ECFA_MVC.Models;
using ECFA_MVC.Models.DTOs;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class ECFADocController : Controller
    {
        private readonly EcfaContext _context;
        public ECFADocController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetECFADocList(string categoryId, int pageNumber = 1, int pageSize = 10)
        {
            // 1. 先建立基礎查詢（此時還不包含 Where 篩選）
            var query = _context.Ecfadocs.AsQueryable();

            // 2. 判斷 categoryId 是否不為空值、Null、"null" 字串 或 "-1"
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                if (int.TryParse(categoryId, out int parsedId))
                {
                    int? categoryIdInt = parsedId;
                    // 只有當 categoryId 有效時，才動態加入 Where 條件
                    query = query.Where(f => f.NtCategory == categoryIdInt);
                }
            }

            var projectedQuery = query
                .OrderByDescending(f => f.NtPubDate)
                .Select(f => new EcfadocListDto
                {
                    ntId = f.NtId,
                    ntCategory = f.NtCategory,
                    ntPubDate = f.NtPubDate,
                    ntTitle = f.NtTitle,
                    ntFileName = f.NtFileName
                });


            // 這裡內含 CountAsync() 與 Skip/Take，修正後速度會縮短至幾毫秒
            var pagedData = await PaginatedList<EcfadocListDto>.CreateAsync(projectedQuery, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }
    }
}
