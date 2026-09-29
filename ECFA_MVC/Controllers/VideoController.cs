using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class VideoController : Controller
    {
        private readonly EcfaContext _context;
        public VideoController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetVideoList(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Videos.AsQueryable();

            query = query.OrderByDescending(f => f.NtPubDate);

            var pagedData = await PaginatedList<Video>.CreateAsync(query, pageNumber, pageSize);

            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }
    }
}
