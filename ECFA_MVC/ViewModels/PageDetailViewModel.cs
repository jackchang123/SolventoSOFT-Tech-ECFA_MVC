namespace ECFA_MVC.ViewModels
{
    public class PageDetailViewModel
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public List<AttachmentViewModel> Attachments { get; set; } = new List<AttachmentViewModel>();
    }
    public class AttachmentViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string FileName { get; set; } = "";

        // 實務上通常會將原始的 byte (ntFileSize) 轉成人類好閱讀的格式（如 1.2 MB）
        public string DisplaySize { get; set; } = "";
        public DateTime? PublishDate { get; set; }
    }

}
