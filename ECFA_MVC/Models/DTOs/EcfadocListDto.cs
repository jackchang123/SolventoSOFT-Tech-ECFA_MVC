namespace ECFA_MVC.Models.DTOs
{
    class EcfadocListDto
    {
        public int ntId { get; set; }
        public int? ntCategory { get; set; }
        public DateTime? ntPubDate { get; set; }
        public string ntTitle { get; set; } = "";
        public string ntFileName { get; set; } = "";
    }
}
