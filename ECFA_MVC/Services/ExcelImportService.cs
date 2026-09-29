using ECFA_MVC.Models;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

public class ExcelImportService : IExcelImportService
{
    private readonly EcfaContext _context;

    private readonly IWebHostEnvironment _webHostEnvironment;

    public ExcelImportService(EcfaContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;

        _webHostEnvironment = webHostEnvironment;

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    // 核心方法：現在接受動態傳入的 fileName
    public async Task<int> ImportFixedPathExcelAsync(string fileName, int[] mergedColumnIndices, string tableName, string[] dbColumnNames)
    {
        string projectRoot = _webHostEnvironment.ContentRootPath;

        // 2. 使用 ".." 讓路徑回退一層，精準定位到與專案並排的 EcfaAttachment 資料夾
        // Path.Combine 會自動幫你把 ".." 處理成正確的上一層路徑
        string fullPath = Path.Combine(projectRoot, "..", "EcfaAttachment", "ECFADoc", fileName);

        // 3. 為了保險起見，可以透過 Path.GetFullPath 將其轉換為絕對路徑（這步會把 ".." 消除掉）
        fullPath = Path.GetFullPath(fullPath);

        // 1. 檢查檔案是否存在
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"找不到指定的 Excel 檔案，路徑：{fullPath}");
        }

        await ClearTableWithEfAsync(tableName);

        DataTable dt;

        // 3. 安全開啟檔案串流（支援讀取他人開啟中的 Excel 快照）
        using (var stream = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // 使用官方推薦的大標題跳過配置
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true, // 啟用欄位標頭識別

                        // 🌟 核心修正一：指定第 3 行為真正的「欄位名稱列」
                        ReadHeaderRow = (rowReader) => {
                            rowReader.Read(); // 跳過第 1 行大標題
                            rowReader.Read(); // 跳過第 2 行大標題
                                              // 此時剛好停在第 3 行真正的欄位名稱 (SK, TW_CODE 等)，套件會自動將此行辨識為欄名
                        },

                        // 🌟 核心修正二：過濾掉第 4 行以前的所有內容（確保資料列從第 4 行才開始計數）
                        FilterRow = (rowReader) => {
                            // rowReader.Depth 代表目前讀到第幾行 (索引從 0 開始算)
                            // Depth 0 = 第 1 行, Depth 1 = 第 2 行, Depth 2 = 第 3 行 (標頭欄位)
                            // 我們只保留 Depth 大於 2 的資料（即第 4 行及之後的真實資料）
                            if (rowReader.Depth <= 2)
                            {
                                return false; // 回傳 false 代表徹底丟棄前三行，不排進真實資料列
                            }
                            return true; // 回傳 true 代表這是要寫入資料庫的有效資料
                        }
                    }
                });

                dt = result.Tables[0]; // 精準取得第一個工作表（Sheet）
            }
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            return 0;
        }
        RemoveBottomTrashRows(dt);
        // 3. 處理合併儲存格
        FillMergedCells(dt, mergedColumnIndices);

        SanitizeDataTable(dt);

        // 4. 將資料批次塞入 SQL Server
        int rowsCopied = await WriteToSqlServerAsync(dt, tableName, dbColumnNames);
        return rowsCopied;
    }

    private void FillMergedCells(DataTable dt, int[] columnIndices)
    {
        foreach (int colIdx in columnIndices)
        {
            if (colIdx < 0 || colIdx >= dt.Columns.Count) continue;

            string lastValidValue = string.Empty;

            foreach (DataRow row in dt.Rows)
            {
                var cellValue = row[colIdx];

                if (cellValue == DBNull.Value || string.IsNullOrWhiteSpace(cellValue.ToString()))
                {
                    row[colIdx] = lastValidValue;
                }
                else
                {
                    lastValidValue = cellValue.ToString();
                }
            }
        }
    }
    /// <summary>
    /// 使用 EF Core 清空目標資料表
    /// </summary>
    private async Task ClearTableWithEfAsync(string tableName)
    {
        // 作法一：如果你只有資料表名稱的「字串」，使用內建的 ExecuteSqlRawAsync
        // 這是最安全的動態清空方式，同時兼顧 EF 的連線管理
        string sql = $"IF OBJECT_ID('{tableName}', 'U') IS NOT NULL TRUNCATE TABLE [{tableName}];";

        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
        catch (SqlException ex) when (ex.Number == 4712) // 遇到外鍵約束無法 TRUNCATE
        {
            // 降級改用 DELETE
            string fallbackSql = $"DELETE FROM [{tableName}];";
            await _context.Database.ExecuteSqlRawAsync(fallbackSql);
        }

        /* 
        💡 補充【作法二】：如果你的 Controller 未來改傳「EF 實體物件」而不是字串，
        那就能完全不寫任何 SQL 指令，寫法如下（以強型別的 TaiwanProducts 為例）：
        
        await _context.TaiwanProducts.ExecuteDeleteAsync(); 
        */
    }
    /// <summary>
    /// 在合併儲存格填滿前，由下往上檢查：第 10 欄（Index 10）若是空的就整行刪除，一旦抓到有值就立刻停止
    /// </summary>
    private void RemoveBottomTrashRows(DataTable dt)
    {
        // 🌟 核心修正：固定檢查「第 10 欄」（程式內部的 Index 10 代表 Excel 從左邊數過來的第 11 欄）
        int targetColumnIndex = 9;

        // 防呆：確保 Excel 檔案實際的欄位數量確實有大於 10 欄，否則直接跳出
        if (dt.Columns.Count <= targetColumnIndex)
        {
            return;
        }

        // 🌟 核心修正：由下往上（倒序迴圈）檢查
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            DataRow row = dt.Rows[i];
            var targetValue = row[targetColumnIndex];

            // 🔍 檢查條件：如果這一行的第 10 欄是空的（DBNull、空字串或只有空格）
            if (targetValue == DBNull.Value || string.IsNullOrWhiteSpace(targetValue.ToString()))
            {
                // 🎯 認定它是最底部的備註雜質，直接整行在記憶體中刪除！
                row.Delete();
            }
            else
            {
                // 🎯 關鍵邏輯：一旦抓到有值，代表「已經進入正式資料區」，立刻大功告成跳出迴圈（Stop）！
                break;
            }
        }

        // 正式刷新 DataTable，將剛才標記 Delete 的雜質列從記憶體中完全抹除
        dt.AcceptChanges();
    }
    private void SanitizeDataTable(DataTable dt)
    {
        foreach (DataRow row in dt.Rows)
        {
            foreach (DataColumn col in dt.Columns)
            {
                if (row[col] != DBNull.Value && string.IsNullOrWhiteSpace(row[col].ToString()))
                {
                    row[col] = DBNull.Value;
                }
            }
        }
    }
    private async Task<int> WriteToSqlServerAsync(DataTable dt, string tableName, string[] dbColumnNames)
    {
        int rowsCopiedCount = 0;

        // 從 EF Core 取得目前的資料庫連線字串
        string connectionString = _context.Database.GetConnectionString();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.BatchSize = 5000;
                bulkCopy.BulkCopyTimeout = 60;

                bulkCopy.SqlRowsCopied += (sender, e) => { rowsCopiedCount = (int)e.RowsCopied; };
                bulkCopy.NotifyAfter = dt.Rows.Count;

                // 🌟 改用外部傳進來的參數順序進行對應
                for (int i = 0; i < dbColumnNames.Length; i++)
                {
                    if (i < dt.Columns.Count)
                    {
                        // 參數 1：Excel 從左到右的第 i 欄
                        // 參數 2：你指定的資料庫欄位名稱
                        bulkCopy.ColumnMappings.Add(i, dbColumnNames[i]);
                    }
                }

                await bulkCopy.WriteToServerAsync(dt);
            }
        }

        return rowsCopiedCount == 0 ? dt.Rows.Count : rowsCopiedCount;
    }
}
