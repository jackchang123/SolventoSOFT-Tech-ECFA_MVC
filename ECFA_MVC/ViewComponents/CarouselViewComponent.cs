using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECFA_MVC.Models;

namespace ECFA_MVC.ViewComponents
{
    public class CarouselViewComponent : ViewComponent
    {
        private readonly EcfaContext _context;
        public CarouselViewComponent(EcfaContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string category = "Carousel")
        {
            string Type = "";
            switch (category)
            {
                case "Carousel":
                    Type = "3";
                    break;
                case "Outlink":
                    Type = "2";
                    break;
                case "Link":
                    Type = "1";
                    break;
            }
            var bannerList = await _context.Links.Where(l => l.Type == Type && l.Publish==true).OrderBy(l => l.Sort).ToListAsync();

            return View(category,bannerList);
        }
    }
}
