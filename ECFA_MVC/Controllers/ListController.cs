using ECFA_MVC.Models;
using ECFA_MVC.Models.DTOs;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class ListController : Controller
    {
        private readonly EcfaContext _context;
        public ListController(EcfaContext context)
        {
            _context = context;
        }
        public IActionResult Event(int nid)
        {
            var Node = _context.Ecfamenus.FirstOrDefault(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";
            return View();
        }
        public IActionResult Video(int nid)
        {
            var Node = _context.Ecfamenus.FirstOrDefault(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";
            return View();
        }
        public IActionResult News(int nid)
        {
            var Node = _context.Ecfamenus.FirstOrDefault(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";
            return View();
        }
        public IActionResult Service(int nid)
        {
            var Node = _context.Ecfamenus.FirstOrDefault(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";
            var targetIds = new List<int> { 1, 7, 24 };

            // 1. 撈出所有相關資料（包含父與子）
            var rawData = _context.Services
                .Where(x => targetIds.Contains(x.NtId) || targetIds.Contains(x.NtParentId))
                .Select(x => new
                {
                    x.NtId,
                    x.NtParentId,
                    x.NtTitle
                })
                .ToList();

            // 2. 在記憶體中組裝成階層結構
            List<ServiceGroupDto> result = rawData
                .Where(x => targetIds.Contains(x.NtId)) // 先過濾出大標題 (ntId 1, 7, 24)
                .Select(p => new ServiceGroupDto
                {
                    ntId = p.NtId,
                    ntTitle = p.NtTitle,
                    // 尋找 ntParentId 等於當前 ntId 的子項目
                    Children = rawData
                        .Where(c => c.NtParentId == p.NtId)
                        .Select(c => new ServiceItemDto
                        {
                            ntId = c.NtId,
                            ntTitle = c.NtTitle
                        }).ToList()
                }).ToList();

            return View(result);
        }
        public async Task<IActionResult> FAQ(int nid)
        {
            // 這裡也可以改用非同步的 FirstOrDefaultAsync
            var Node = await _context.Ecfamenus.FirstOrDefaultAsync(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";

            // 【關鍵修正】這裡必須加上 await 關鍵字！
            var categories = await _context.Faqs
                .Where(c => c.NtParentId == -1)
                .OrderBy(c => c.NtId)
                .Select(c => new SelectListItem
                {
                    Value = c.NtId.ToString(),
                    Text = c.NtTitle
                })
                .ToListAsync();

            // 2. 這時候帶過去的才是真正的 List<SelectListItem>
            ViewData["Categories"] = categories;

            return View();
        }

        public async Task<IActionResult> ATSFAQ(int nid)
        {
            // 這裡也可以改用非同步的 FirstOrDefaultAsync
            var Node = await _context.Ecfamenus.FirstOrDefaultAsync(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";

            // 【關鍵修正】這裡必須加上 await 關鍵字！
            var categories = await _context.Atsfaqs
                .Where(c => c.NtParentId == -1)
                .OrderBy(c => c.NtId)
                .Select(c => new SelectListItem
                {
                    Value = c.NtId.ToString(),
                    Text = c.NtTitle
                })
                .ToListAsync();

            // 2. 這時候帶過去的才是真正的 List<SelectListItem>
            ViewData["Categories"] = categories;

            return View();
        }

        public async Task<IActionResult> DmadList(int nid , int c)
        {
            // 這裡也可以改用非同步的 FirstOrDefaultAsync
            var Node = await _context.Ecfamenus.FirstOrDefaultAsync(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";

            return View();
        }
        public async Task<IActionResult> DownloadDoc(int nid)
        {
            // 這裡也可以改用非同步的 FirstOrDefaultAsync
            var Node = await _context.Ecfamenus.FirstOrDefaultAsync(n => n.NodeId == nid);
            ViewBag.TitleName = Node?.NodeName ?? "標題";

            return View();
        }
    }
}
