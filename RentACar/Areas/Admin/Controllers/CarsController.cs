﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.DataContext;
using RentACar.DataContext.Entities;
using RentACar.Areas.Admin.Models;
using RentACar.Areas.Admin.Data;
using RentACar.Areas.Admin.Extensions;
using RentACar.DataContext.Entities.RentACar.DataContext.Entities;

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
            var model = new CarCreateViewModel
            {
                Name = string.Empty,
                Seats = null,
                Categories = GetCategorySelectList()
            };
            return View(model);
        }

        //[HttpPost]
        /*   public async Task<IActionResult> Create(CarCreateViewModel model)
           {
               if (!ModelState.IsValid)
               {
                   model.Categories = GetCategorySelectList();
                   return View(model);
               }

               var uniqueImageFileName = await model.ImageFile.GenerateFile(FilePathConstants.CarPath);

               var car = new Car
               {
                   Name = model.Name,
                   Seats = model.Seats,
                   Doors = model.Doors,
                   Luggage = model.Luggage,
                   PricePerDay = model.PricePerDay,
                   ImageUrl = uniqueImageFileName,
                   CategoryId = model.CategoryId,
                   Images = new List<CarImage>(),
               };

               _context.Cars.Add(car);
               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
           }


           public async Task<IActionResult> Edit(int id)
           {
               var car = await _context.Cars.FindAsync(id);
               if (car == null) return NotFound();

               var model = new CarCreateViewModel
               {
                   Id = car.Id,
                   Name = car.Name,
                   Seats = car.Seats,
                   Doors = car.Doors,
                   Luggage = car.Luggage,
                   PricePerDay = car.PricePerDay,
                   ImageUrl = car.ImageUrl,
                   CategoryId = car.CategoryId,
                   Categories = GetCategorySelectList()
               };

               return View(model);
           }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategorySelectList();
                return View(model);
            }

            if (model.CoverImageFile == null || !model.CoverImageFile.IsImage() || !model.CoverImageFile.IsAllowedSize(1))
            {
                ModelState.AddModelError("ImageFile", "Əsas şəkil düzgün seçilməlidir.");
                model.Categories = GetCategorySelectList();
                return View(model);
            }

            var carImages = new List<CarImage>();

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles)
                {
                    if (!file.IsImage() || !file.IsAllowedSize(1))
                    {
                        ModelState.AddModelError("CarImageFiles", $"{file.FileName} şəkil olmalıdır və 1 MB-dan böyük olmamalıdır.");
                        model.Categories = GetCategorySelectList();
                        return View(model);
                    }
                }

                foreach (var file in model.ImageFiles)
                {
                    var uniqueFileName = await file.GenerateFile(FilePathConstants.CarPath);
                    carImages.Add(new CarImage { ImageUrl= uniqueFileName});
                }
            }

            var coverImageName = await model.CoverImageFile.GenerateFile(FilePathConstants.CarPath);

            var car = new Car
            {
                Name = model.Name,
                PricePerDay = model.PricePerDay,
                Seats = model.Seats ?? 0,
                Doors = model.Doors ?? 0,
                Luggage = model.Luggage ?? 0,
                CategoryId = model.CategoryId,
                ImageUrl = coverImageName,
                Description = model.Description,
                Images = carImages
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        public async Task<IActionResult> Edit(CarCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = GetCategorySelectList();
                return View(model);
            }

            var car = await _context.Cars.FindAsync(model.Id);
            if (car == null) return NotFound();

            if (model.CoverImageFile != null)
            {
                car.ImageUrl = await model.CoverImageFile.GenerateFile(FilePathConstants.CarPath);
            }

            car.Name = model.Name;
            car.Seats = model.Seats;
            car.Doors = model.Doors;
            car.Luggage = model.Luggage;
            car.PricePerDay = model.PricePerDay;
            car.CategoryId = model.CategoryId;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null) return NotFound();

            var model = new CarCreateViewModel
            {
                Id = car.Id,
                Name = car.Name,
                Seats = car.Seats,
                Doors = car.Doors,
                Luggage = car.Luggage,
                PricePerDay = car.PricePerDay,
                CoverImageUrl = car.ImageUrl,
                CategoryId = car.CategoryId,
                Categories = GetCategorySelectList()                
            };

            return View(model);
        }

         private List<SelectListItem> GetCategorySelectList()
        {
            return _context.Categories
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name })
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Avtomobil uğurla silindi.";
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Route("Admin/Cars/DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
