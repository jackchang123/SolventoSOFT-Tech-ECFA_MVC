namespace ECFA_MVC.ViewModels
{
    public class MenuViewModel
    {
        public int NodeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = "#";
        public string Target { get; set; } = "_self"; // 新增：用來控制外站開新視窗
        public List<MenuViewModel> SubMenus { get; set; } = new();
    }
}
