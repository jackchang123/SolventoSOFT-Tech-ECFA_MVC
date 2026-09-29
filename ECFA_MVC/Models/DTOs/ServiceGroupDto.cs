namespace ECFA_MVC.Models.DTOs
{
    public class ServiceItemDto
    {
        public int ntId { get; set; }
        public string ntTitle { get; set; }
    }

    public class ServiceGroupDto
    {
        public int ntId { get; set; }
        public string ntTitle { get; set; }
        public List<ServiceItemDto> Children { get; set; } = new List<ServiceItemDto>();
    }
}
