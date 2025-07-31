using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.DataContext;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CarController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            var cars = await _dbContext.Cars.Include(c => c.Category).ToListAsync();

            var minPrice = await _dbContext.Cars.MinAsync(c => c.PricePerDay);
            var maxPrice = await _dbContext.Cars.MaxAsync(c => c.PricePerDay);

            var model = new CarsViewModel
            {
                Categories = categories,
                Cars = cars,
                MinPrice = minPrice,
                MaxPrice = maxPrice
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Filter([FromBody] CarFilterRequest filter)
        {
            var query = _dbContext.Cars
                .Include(c => c.Category)
                .AsQueryable();

            if (filter.BodyTypes != null && filter.BodyTypes.Any())
            {
                // BodyTypes əslində Categories-dirsə, bu belə olmalıdır:
                query = query.Where(c => filter.BodyTypes.Contains(c.CategoryId));
            }

            if (!string.IsNullOrEmpty(filter.SearchKeyword))
            {
                var keyword = filter.SearchKeyword.ToLower();
                query = query.Where(c =>
                    c.Name.ToLower().Contains(keyword) ||
                    c.Category.Name.ToLower().Contains(keyword)
                );
            }

            query = query.Where(c =>
                c.PricePerDay >= filter.MinPrice &&
                c.PricePerDay <= filter.MaxPrice);

            var filteredCars = await query.ToListAsync();

            return PartialView("_CarCardsPartial", filteredCars);
        }
        public async Task<IActionResult> Details(int id)
        {
            var car = await _dbContext.Cars
                .Include(c => c.Category)
                .Include(c => c.Images)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }


    }
}
