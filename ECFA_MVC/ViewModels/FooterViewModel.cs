namespace ECFA_MVC.ViewModels
{
    public class FooterViewModel
    {
        // 瀏覽人數（格式化後的字串）
        public string VisitorCount { get; set; } = string.Empty;

        // 系統最後更新時間
        public string LastUpdateTime { get; set; } = string.Empty;
    }
}
