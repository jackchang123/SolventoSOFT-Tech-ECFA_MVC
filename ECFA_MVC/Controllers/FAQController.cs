using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class FAQController : Controller
    {
        private readonly EcfaContext _context;
        public FAQController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetFAQList(string categoryId, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Faqs.AsQueryable();

            if (!string.IsNullOrEmpty(categoryId) && categoryId != "-1")
                query = query.Where(f => f.NtParentId.ToString() == categoryId);
            else
                query = query.Where(f => f.NtParentId != -1);

            query = query.OrderByDescending(f => f.NtPubDate);

            var pagedData = await PaginatedList<Faq>.CreateAsync(query, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }

        [HttpGet]
        public async Task<IActionResult> ShowFAQ(int nid, int id)
        {
            var pageData = await _context.Faqs
                .FirstOrDefaultAsync(p => p.NtId == id);

            if (pageData == null)
            {
                return NotFound(); // 若找不到網頁，回傳 404
            }

            var viewModel = new PageDetailViewModel
            {
                Title = pageData.NtTitle,     // 假設 Pages 表中的標題欄位叫 ntTitle
                Content = pageData.NtContent ?? string.Empty, // 假設 Pages 表中的內文欄位叫 ntContent
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ShowATSFAQ(int nid, int id)
        {
            var pageData = await _context.Atsfaqs
                .FirstOrDefaultAsync(p => p.NtId == id);

            if (pageData == null)
            {
                return NotFound(); // 若找不到網頁，回傳 404
            }

            var viewModel = new PageDetailViewModel
            {
                Title = pageData.NtTitle,     // 假設 Pages 表中的標題欄位叫 ntTitle
                Content = pageData.NtContent, // 假設 Pages 表中的內文欄位叫 ntContent
            };

            return View(viewModel);
        }

    }
}
