using Microsoft.AspNetCore.Mvc;

namespace RentACar.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); // Views/Shared/Components/Header/Default.cshtml
        }
    }
}
