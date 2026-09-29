using System.Collections.Generic;

public interface IMenuUrlService
{
    // 獲取選單對照表字典
    Dictionary<string, string> GetNodeIdDictionary();

    // 依據傳入參數與對照表生成現代化網址
    string GenerateUrl(string tblName, string ntId, string ntParentId, Dictionary<string, string> dicNode);
}