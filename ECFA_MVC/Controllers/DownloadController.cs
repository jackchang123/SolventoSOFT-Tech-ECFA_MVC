using ECFA_MVC.Helpers; // 引用您的 Helper
using ECFA_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ECFA_MVC.Controllers;

[ApiController]
[Route("api/download")]
public class DownloadController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly EcfaContext _context;
    private readonly string _storageRoot;
    public DownloadController(IConfiguration configuration, EcfaContext context)
    {
        _configuration = configuration;
        _context = context;
        _storageRoot = configuration.GetSection("FileStorageSetting")["RootPath"] ?? string.Empty;
    }
    private class FileResultData
    {
        public string? FileName { get; set; }
        public byte[]? FileBytes { get; set; }
        public string? FileSize { get; set; } 
    }
    [HttpGet("file")]
    public IActionResult Download([FromQuery] string path)
    {
        // 直接呼叫 Helper 進行安全檢查
        if (!FileSecurityHelper.TryGetSafeFilePath(_storageRoot, path,
                out string fullPath, out string fileName, out string contentType))
        {
            // 為了安全性，統一回傳 NotFound，不對外通報是「路徑不合法」還是「真的找不到檔案」
            return NotFound("找不到該檔案。");
        }

        // 安全放行，直接回傳檔案
        return PhysicalFile(fullPath, contentType, fileDownloadName: fileName, enableRangeProcessing: true);
    }
    /// <summary>
    /// 統一檔案下載端點
    /// 網址對應：GET /api/download?Id=123&T=DMAd&Type=2
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> DownloadFile(
        [FromQuery] string? Id,
        [FromQuery] string? T,
        [FromQuery] string? Type)
        { 
        // 1. 驗證 ID 是否為合法整數
        if (!int.TryParse(Id, out int ntId))
        {
            return ReturnHtmlErrorMessage();
        }
        switch (T.ToLower())
        {
            case "dmad":
            {
                var query = _context.Dmads.AsQueryable();
                var fileData = await query.Where(x => x.NtId == ntId)
                    .Select(x => Type == "2" ? new FileResultData
                    {
                        FileName = x.NtFileName2,
                        FileBytes = x.NtFile2,
                        FileSize = x.NtFileSize2
                    }
                    : new FileResultData
                    {
                        FileName = x.NtFileName,
                        FileBytes = x.NtFile,
                        FileSize = x.NtFileSize
                    })
                    .FirstOrDefaultAsync();
                if (fileData != null && fileData.FileBytes != null && !string.IsNullOrEmpty(fileData.FileName))
                {
                    // 優化：如果資料庫有存精準的 byte 數字字串，主動補上 Content-Length 標頭
                    // 這可以確保在極端網路環境下，瀏覽器的下載進度條 100% 正常顯示，且防範 proxy 快取錯誤
                    if (long.TryParse(fileData.FileSize, out long parsedSize))
                    {
                        Response.Headers.ContentLength = parsedSize;
                    }
                    else
                    {
                        // 如果您的資料庫存的是 "1.2 MB" 這種文字，就由 .NET 10 自動依據 Byte 陣列長度去抓
                        Response.Headers.ContentLength = fileData.FileBytes.Length;
                    }

                    var fileStream = new MemoryStream(fileData.FileBytes);

                    return File(
                        fileStream: fileStream,
                        contentType: "application/octet-stream",
                        fileDownloadName: fileData.FileName,
                        enableRangeProcessing: true // 斷點續傳非常依賴上面正確的 ContentLength！
                    );
                }
                break;
            }
            case "ecfadoc":
                {
                    var query = _context.Ecfadocs.AsQueryable();
                    var fileData = await query.Where(x => x.NtId == ntId)
                        .Select(x => new FileResultData
                        {
                            FileName = x.NtFileName,
                            FileBytes = x.NtFile,
                            FileSize = x.NtFileSize
                        })
                        .FirstOrDefaultAsync();
                    if (fileData != null && fileData.FileBytes != null && !string.IsNullOrEmpty(fileData.FileName))
                    {
                        // 優化：如果資料庫有存精準的 byte 數字字串，主動補上 Content-Length 標頭
                        // 這可以確保在極端網路環境下，瀏覽器的下載進度條 100% 正常顯示，且防範 proxy 快取錯誤
                        if (long.TryParse(fileData.FileSize, out long parsedSize))
                        {
                            Response.Headers.ContentLength = parsedSize;
                        }
                        else
                        {
                            // 如果您的資料庫存的是 "1.2 MB" 這種文字，就由 .NET 10 自動依據 Byte 陣列長度去抓
                            Response.Headers.ContentLength = fileData.FileBytes.Length;
                        }

                        var fileStream = new MemoryStream(fileData.FileBytes);

                        return File(
                            fileStream: fileStream,
                            contentType: "application/octet-stream",
                            fileDownloadName: fileData.FileName,
                            enableRangeProcessing: true // 斷點續傳非常依賴上面正確的 ContentLength！
                            );
                    }
                    break;
                }
            case "service":
                {
                    var query = _context.Services.AsQueryable();
                    var fileData = await query.Where(x => x.NtId == ntId)
                        .Select(x => new FileResultData
                        {
                            FileName = x.NtFileName,
                            FileBytes = x.NtFile,
                            FileSize = x.NtFileSize
                        })
                        .FirstOrDefaultAsync();
                    if (fileData != null && fileData.FileBytes != null && !string.IsNullOrEmpty(fileData.FileName))
                    {
                        // 優化：如果資料庫有存精準的 byte 數字字串，主動補上 Content-Length 標頭
                        // 這可以確保在極端網路環境下，瀏覽器的下載進度條 100% 正常顯示，且防範 proxy 快取錯誤
                        if (long.TryParse(fileData.FileSize, out long parsedSize))
                        {
                            Response.Headers.ContentLength = parsedSize;
                        }
                        else
                        {
                            // 如果您的資料庫存的是 "1.2 MB" 這種文字，就由 .NET 10 自動依據 Byte 陣列長度去抓
                            Response.Headers.ContentLength = fileData.FileBytes.Length;
                        }

                        var fileStream = new MemoryStream(fileData.FileBytes);

                        return File(
                            fileStream: fileStream,
                            contentType: "application/octet-stream",
                            fileDownloadName: fileData.FileName,
                            enableRangeProcessing: true // 斷點續傳非常依賴上面正確的 ContentLength！
                            );
                    }
                    break;
                }

            // ... 其他資料表依此類推 ...
            default:
                return ReturnHtmlErrorMessage();
        }


        // 若查無資料或發生異常，回傳錯誤訊息
        return ReturnHtmlErrorMessage();
    }

    /// <summary>
    /// 統一封裝舊系統要求的 HTML 錯誤提示訊息
    /// </summary>
    private ContentResult ReturnHtmlErrorMessage()
    {
        string errorHtml = "您好：由於非正常操作造成系統無法存取頁面，若您的操作為正常行為，卻顯示此頁面時，請說明操作步驟並留下您的聯絡電話至 E-MAIL 至 (tradeweb@trade.gov.tw)，我們會立即處理，感謝您。";

        return Content(errorHtml, "text/html", System.Text.Encoding.UTF8);
    }
}
