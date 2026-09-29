namespace ECFA_MVC.Models.DTOs
{
    class DMAdListDto
    {
        public int ntId { get; set; }
        public int? ntCategory { get; set; }
        public DateTime? ntPubDate { get; set; }
        public string ntTitle { get; set; } = "";
        public string ntFileName { get; set; } = "";
        public string ntFileName2 { get; set; } = "";
    }
}
