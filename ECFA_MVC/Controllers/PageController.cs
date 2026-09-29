using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Controllers
{
    public class PageController : Controller
    {
        private readonly EcfaContext _context;
        public PageController(EcfaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> RelatedDoc(int nid)
        {
            // 1. 讀取 Pages 資料表中的主文章
            var relatedDocData = await _context.RelatedDocs
                .FirstOrDefaultAsync(p => p.NtNodeId == nid);

            if (relatedDocData == null)
            {
                return NotFound(); // 若找不到網頁，回傳 404
            }

            return View(relatedDocData);
        }

        [HttpGet]
        public async Task<IActionResult> ShowDetail(int nid,int pid)
        {
            // 1. 讀取 Pages 資料表中的主文章
            var pageData = await _context.Pages
                .FirstOrDefaultAsync(p => p.NtId == pid);

            if (pageData == null)
            {
                return NotFound(); // 若找不到網頁，回傳 404
            }

            // 2. 利用主文章的 ntId 去關聯查詢附件，並依 ntSort 排序
            var attachmentsData = await _context.PageAttachments
                .Where(a => a.NtParentId == pid)
                .OrderBy(a => a.NtSort)
                .ToListAsync();

            // 3. 組裝成前端專用的 PageDetailViewModel
            var viewModel = new PageDetailViewModel
            {
                Title = pageData.NtTitle,     // 假設 Pages 表中的標題欄位叫 ntTitle
                Content = pageData.NtContent, // 假設 Pages 表中的內文欄位叫 ntContent
                Attachments = attachmentsData.Select(a => new AttachmentViewModel
                {
                    Id = a.NtId,
                    Title = a.NtTitle,
                    FileName = a.NtFileName,
                    // 將字串型態的資料庫大小直接傳遞（或在此寫公式轉成人類閱讀的 KB/MB）
                    DisplaySize = a.NtFileSize,
                    PublishDate = a.NtPubDate
                }).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// 下載資料庫 varbinary(MAX) 的實體檔案
        /// URL 範例: /Page/DownloadAttachment/12
        /// </summary>
        public async Task<IActionResult> DownloadAttachment(int id)
        {
            // 1. 從資料庫撈出該筆附件
            var attachment = await _context.PageAttachments
                .FirstOrDefaultAsync(a => a.NtId == id);

            if (attachment == null || attachment.NtFile == null)
            {
                return NotFound(); // 找不到檔案回傳 404
            }

            // 2. 準備下載所需的資訊
            byte[] fileBytes = attachment.NtFile; // 這是你的 varbinary(MAX) 資料
            string fileName = attachment.NtFileName ?? "download.file";

            // 3. 根據副檔名自動判斷 MimeType（MimeMapping）
            // 在 .NET Core 中最安全、最標準的萬用下載型態是 "application/octet-stream"
            string contentType = "application/octet-stream";

            // 4. 使用內建的 File 方法，它會讓瀏覽器跳出「另存新檔」視窗
            return File(fileBytes, contentType, fileName);
        }

    }
}
