using ECFA_MVC.Models;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;

namespace ECFA_MVC.Helpers;

public static class FileSecurityHelper
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

    /// <summary>
    /// 驗證子路徑是否安全，並回傳完整的實體路徑與檔案資訊
    /// </summary>
    public static bool TryGetSafeFilePath(
        string rootPath,
        string subPath,
        out string fullPath,
        out string fileName,
        out string contentType)
    {
        fullPath = string.Empty;
        fileName = string.Empty;
        contentType = "application/octet-stream";

        // 1. 基本安全檢查
        if (string.IsNullOrWhiteSpace(subPath) ||
            subPath.Contains("..") ||
            subPath.Contains(":") ||
            subPath.Contains("$"))
        {
            return false;
        }

        // 2. 正規化路徑分隔符號 (確保 Windows/Linux 都能正確組合)
        string normalizedSubPath = subPath.Replace('/', Path.DirectorySeparatorChar)
                                         .Replace('\\', Path.DirectorySeparatorChar);

        // 3. 解析為絕對路徑
        string resolvedPath = Path.GetFullPath(Path.Combine(rootPath, normalizedSubPath));

        // 4. 白名單檢查：確保沒有逃脫根目錄，且檔案確實存在
        if (!resolvedPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) ||
            !File.Exists(resolvedPath))
        {
            return false;
        }

        // 5. 驗證成功，賦值回傳
        fullPath = resolvedPath;
        fileName = Path.GetFileName(resolvedPath);

        if (ContentTypeProvider.TryGetContentType(resolvedPath, out var detectedType))
        {
            contentType = detectedType;
        }

        return true;
    }
}
