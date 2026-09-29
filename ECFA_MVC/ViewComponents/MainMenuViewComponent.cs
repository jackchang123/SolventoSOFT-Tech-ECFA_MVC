using ECFA_MVC.Helpers;
using ECFA_MVC.Models;
using ECFA_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECFA_MVC.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly EcfaContext _context;

        public MainMenuViewComponent(EcfaContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default", string nodeId = "")
        {
            if (viewName == "Breadcrumbs" || viewName == "LeftMenu")
            {
                // 確保 nodeId 有值且可以正確轉為數字
                if (int.TryParse(nodeId, out int currentNodeId))
                {
                    // 呼叫新寫的由下往上找父層的方法
                    var breadcrumbData = await GetBreadcrumbDataAsync(currentNodeId);

                    if (viewName == "LeftMenu")
                    {
                        List<int> activeNodeIds = breadcrumbData.Select(m => m.NodeId).ToList();
                        ViewBag.ActiveNodeIds = activeNodeIds;
                    }
                    else
                        return View(viewName, breadcrumbData);
                }
                else
                    return View(viewName, new List<MenuViewModel>());
            }

            // 模擬從資料庫或 API 取得選單資料
            string rootNodeId = "";
            if (nodeId != "")
                rootNodeId = FindRootId(nodeId);
            var menuData = await GetMenuDataAsync(rootNodeId);

            return View(viewName, menuData);
        }
        string FindRootId(string nodeId)
        {
            var currentNode = _context.Ecfamenus.FirstOrDefault(n => n.NodeId.ToString() == nodeId);

            // 💡 關鍵防呆：如果找不到該節點，直接回傳傳入的 nodeId 避免崩潰
            if (currentNode == null)
            {
                return nodeId;
            }

            // 如果已經沒有父節點，代表目前就是最上層
            if (currentNode.ParentId == 0)
            {
                return currentNode.NodeId.ToString();
            }

            // 繼續往上找
            return FindRootId(currentNode.ParentId.ToString());
        }
        private async Task<List<MenuViewModel>> GetBreadcrumbDataAsync(int currentNodeId)
        {
            // 1. 先從資料庫取出所有啟用的選單（載入到記憶體中方便快速比對）
            var rawMenus = await _context.Ecfamenus
                .Where(a => a.Block == false)
                .ToListAsync();

            var breadcrumbPath = new List<Ecfamenu>();

            // 2. 找到目前所在的節點作為搜尋起點
            var currentNode = rawMenus.FirstOrDefault(m => m.NodeId == currentNodeId);

            // 3. 由下往上追溯父節點，直到找不到或到達頂層 (ParentId == 0)
            while (currentNode != null)
            {
                // 將目前的節點轉成 MenuViewModel 並塞入清單
                breadcrumbPath.Add(currentNode);

                // 關鍵：將 currentNode 切換為它的父節點
                currentNode = rawMenus.FirstOrDefault(m => m.NodeId == currentNode.ParentId);
            }

            // 4. 因為是「由下往上」抓取（當前頁面 -> 父層 -> 祖父層），所以最後要將陣列「反轉」
            breadcrumbPath.Reverse();

            var menuTree = breadcrumbPath
                .Select(m => BuildMenuTree(m, breadcrumbPath))
                .ToList();

            return menuTree;
        }
        private async Task<List<MenuViewModel>> GetMenuDataAsync(string rootNodeId)
        {
            // 1. 先從資料庫取出所有啟用的選單
            var rawMenus = await _context.Ecfamenus
                .Where(a => a.Block == false)
                .OrderBy(a => a.Sort)
                .ToListAsync();

            List<Ecfamenu> startNodes;

            // 2. 判斷 rootNodeId 是否有值
            if (!string.IsNullOrEmpty(rootNodeId) && int.TryParse(rootNodeId, out int parsedId))
            {
                // 有特定節點：起點就是該節點本身（自成一個清單）
                startNodes = rawMenus.Where(m => m.NodeId == parsedId).ToList();
            }
            else
            {
                // 沒有特定節點：起點就是所有最頂層的選單（ParentId == 0）
                startNodes = rawMenus.Where(m => m.ParentId == 0).ToList();
            }

            // 3. 呼叫遞迴方法，從起點節點開始建立樹狀結構
            var menuTree = startNodes
                .Select(m => BuildMenuTree(m, rawMenus))
                .ToList();

            return menuTree;
        }
        private MenuViewModel BuildMenuTree(Ecfamenu currentNode, List<Ecfamenu> allMenus)
        {
            string? targetValue = currentNode.PrgTarget?.ToLower() switch
            {
                "_top" => "_top",
                "_new" => "_blank", // 網頁標準中，開新視窗通常對應 _blank
                _ => null       // 其他情況（如 Null）設為 null 
            };
            return new MenuViewModel
            {
                NodeId = currentNode.NodeId,
                Title = currentNode.NodeName,
                Url = MenuUrlHelper.GetNodeUrl(currentNode),
                // 關鍵：在 SubMenus 內部再度呼叫自己（BuildMenuTree）
                Target = targetValue ?? string.Empty,
                SubMenus = allMenus
                    .Where(sub => sub.ParentId == currentNode.NodeId)
                    .Select(sub => BuildMenuTree(sub, allMenus)) // 遞迴呼叫
                    .ToList()
            };
        }
    }
}