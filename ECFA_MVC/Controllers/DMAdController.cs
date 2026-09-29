using ECFA_MVC.Models;
using ECFA_MVC.Models.DTOs;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static ECFA_MVC.Controllers.ECFADocController;

namespace ECFA_MVC.Controllers
{
    public class DMAdController : Controller
    {
        private readonly EcfaContext _context;
        public DMAdController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDMAdList(int c, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Dmads.AsQueryable();

            query = query.Where(f => f.NtCategory == c);

            query = query.OrderByDescending(f => f.NtCrtDate);

            var projectedQuery = query
                .OrderByDescending(f => f.NtPubDate)
                .Select(f => new DMAdListDto
                {
                    ntId = f.NtId,
                    ntCategory = f.NtCategory,
                    ntPubDate = f.NtPubDate,
                    ntTitle = f.NtTitle,
                    ntFileName = f.NtFileName,
                    ntFileName2 = f.NtFileName2
                });

            var pagedData = await PaginatedList<DMAdListDto>.CreateAsync(projectedQuery, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }

        //[HttpGet]
        //public async Task<IActionResult> ShowFAQ(int nid, int id)
        //{
        //    var pageData = await _context.Faqs
        //        .FirstOrDefaultAsync(p => p.NtId == id);

        //    if (pageData == null)
        //    {
        //        return NotFound(); // 若找不到網頁，回傳 404
        //    }

        //    var viewModel = new PageDetailViewModel
        //    {
        //        Title = pageData.NtTitle,     // 假設 Pages 表中的標題欄位叫 ntTitle
        //        Content = pageData.NtContent, // 假設 Pages 表中的內文欄位叫 ntContent
        //    };

        //    return View(viewModel);
        //}
    }
}
