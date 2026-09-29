using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // 網址會是: api/excel
    public class ExcelController : ControllerBase
    {
        private readonly IExcelImportService _excelImportService;

        // 透過 DI 依賴注入剛剛寫好的 Service
        public ExcelController(IExcelImportService excelImportService)
        {
            _excelImportService = excelImportService;
        }

        /// <summary>
        /// 透過網址直接觸發固定路徑 Excel 匯入
        /// 網址：GET https://localhost:xxxx/api/excel/import-fixed
        /// </summary>
        [HttpGet("import-codelist")]
        public async Task<IActionResult> TriggerFixedImport()
        {
            try
            {
                // 定義哪些欄位索引含有「合併儲存格」
                int[] mergedColumnIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

                string filenameC = @"ECFA早收清單大陸方面減讓稅號對照台灣方面稅號2026版20251230.xlsx";
                string tableNameC = "CodeList267";
                string[] fileColumnsC = new string[] {
                    "SK", "CN_CODE", "CN_CNAME", "CN_ENAME", "CN_TAX",
                    "EX", "TW_CODE", "TW_CNAME", "TW_ENAME", "TW_TAX", "MEMO"
                };
                int fileRowsC = await _excelImportService.ImportFixedPathExcelAsync(filenameC, mergedColumnIndices, tableNameC, fileColumnsC);

                string filenameT = @"ECFA早收清單台灣方面減讓稅號對照大陸方面稅號2026版20251230.xlsx";
                string tableNameT = "CodeList539";
                string[] fileColumnsT = new string[] {
                    "SK", "TW_CODE", "TW_CNAME", "TW_ENAME", "TW_TAX",
                    "EX", "CN_CODE", "CN_CNAME", "CN_ENAME", "CN_TAX", "MEMO"
                };
                int fileRowsT = await _excelImportService.ImportFixedPathExcelAsync(filenameT, mergedColumnIndices, tableNameT, fileColumnsT);

                // 回傳總體結果
                return Ok(new
                {
                    Success = true,
                    Message = "所有 Excel 檔案已順利匯入各自的資料表！",
                    Details = new[]
                    {
                        new { Table = tableNameC, Path = filenameC, ImportedCount = fileRowsC },
                        new { Table = tableNameT, Path = filenameT, ImportedCount = fileRowsT }
                    },
                    TotalImportedCount = fileRowsT + fileRowsC,
                    ExecuteTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                });
            }
            catch (System.IO.FileNotFoundException ex)
            {
                // 找不到實體檔案的錯誤處理
                return NotFound(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                // 捕捉其他資料庫或格式異常
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "匯入過程中發生未預期的錯誤。",
                    Details = ex.Message
                });
            }
        }
    }
}