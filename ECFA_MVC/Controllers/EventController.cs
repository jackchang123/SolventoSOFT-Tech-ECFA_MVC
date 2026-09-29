using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly EcfaContext _context;
        public EventController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetPagedList(int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Events.AsNoTracking().OrderBy(p => p.NtId);
            var pagedData = await PaginatedList<Event>.CreateAsync(query, pageNumber, pageSize);
            // 回傳 JSON 物件給前端
            return Json(new
            {
                totalCount = pagedData.TotalCount,
                items = pagedData.Items
            });
        }
    }
}
