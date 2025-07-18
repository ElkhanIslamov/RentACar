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

            var model = new CarsViewModel
            {
                Categories = categories,
                Cars = cars
            };
            return View(model);
        }
    }
}
