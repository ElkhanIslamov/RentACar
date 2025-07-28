using Microsoft.AspNetCore.Mvc;

namespace RentACar.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(); // Views/Shared/Components/Footer/Default.cshtml
        }
    }
}
