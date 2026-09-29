public interface IExcelImportService
{
    /// <summary>
    /// 讀取指定路徑的 Excel 檔案並匯入至 SQL Server
    /// </summary>
    /// <param name="filePath">Excel 檔案的實體路徑</param>
    /// <param name="mergedColumnIndices">含有合併儲存格的欄位索引群</param>
    /// <param name="tableName">SQL Server 目的端資料表名稱</param>
    Task<int> ImportFixedPathExcelAsync(string fileName, int[] mergedColumnIndices, string tableName, string[] dbColumnNames);
}
