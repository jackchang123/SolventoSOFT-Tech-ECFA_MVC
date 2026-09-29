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
            if (categoryId1 == "-1")
            {
                query = query.Where(f => f.NtParentId != -1);
            }
            else if (categoryId1 != "-1" && categoryId2 == "-1")
            {
                var categoryIds = await _context.Atsfaqs
                .Where(sub => sub.NtParentId.ToString() == categoryId1)
                .Select(sub => sub.NtId)
                .ToListAsync();

                query = query.Where(f => categoryIds.Contains(f.NtParentId));
            }
            else
            {
                query = query.Where(f => f.NtParentId.ToString() == categoryId2);
            }

            query = query.OrderByDescending(f => f.NtModDate);

            var pagedData = await PaginatedList<Atsfaq>.CreateAsync(query, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }
    }
}
