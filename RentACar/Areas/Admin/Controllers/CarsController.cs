using Microsoft.AspNetCore.Mvc;

namespace RentACar.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;   
       using Microsoft.EntityFrameworkCore;
    using global::RentACar.DataContext.Entities;
    using global::RentACar.DataContext;
    using global::RentACar.Areas.Admin.Models;

    namespace RentACar.Areas.Admin.Controllers
    {
        [Area("Admin")]
        public class CarsController : Controller
        {
            private readonly AppDbContext _context;
            private readonly IWebHostEnvironment _env;

            public CarsController(AppDbContext context, IWebHostEnvironment env)
            {
                _context = context;
                _env = env;
            }

            public IActionResult Index()
            {
                var cars = _context.Cars.Include(c => c.Category).ToList();
                return View(cars);
            }

            public IActionResult Create()
            {
                var model = new CarViewModel
                {
                    Categories = _context.Categories.ToList()
                };
                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> Create(CarViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    model.Categories = _context.Categories.ToList();
                    return View(model);
                }

                string imagePath = "";
                if (model.ImageFile != null)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(model.ImageFile.FileName);
                    string path = Path.Combine(_env.WebRootPath, "uploads", "cars", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }
                    imagePath = $"/uploads/cars/{fileName}";
                }

                var car = new Car
                {
                    Name = model.Name,
                    Seats = model.Seats,
                    Doors = model.Doors,
                    Luggage = model.Luggage,
                    PricePerDay = model.PricePerDay,
                    ImageUrl = imagePath,
                    CategoryId = model.CategoryId
                };

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Edit, Details, Delete metodlarını da əlavə edə bilərik
        }
    }


}
