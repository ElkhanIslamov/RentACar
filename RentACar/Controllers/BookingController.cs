using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.DataContext.Entities;
using RentACar.DataContext;
using Microsoft.AspNetCore.Identity;
using RentACar.Models;
using Microsoft.EntityFrameworkCore;

namespace RentACar.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly AppDbContext _context;

    public BookingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
   public async Task<IActionResult> QuickBooking(int? carId)
    {
        var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == carId);
        var viewModel = new BookingViewModel
        {
            CarId = car?.Id,
            CarType = car?.Name
        };

        return View(viewModel);
    }

    [HttpPost]
   public async Task<IActionResult> QuickBooking(BookingViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

      var booking = new Booking
        {
            CarId = model.CarId,
            CarType = model.CarType,
            CustomerName = model.CustomerName,
            Email = model.Email,
            Phone = model.Phone,
            PickupLocation = model.PickupLocation,
            DropoffLocation = model.DropoffLocation,
            PickupDate = model.PickupDate,
            PickupTime = model.PickupTime,
            ReturnDate = model.ReturnDate,
            ReturnTime = model.ReturnTime,
            CreatedAt = DateTime.Now,
            UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
        };


        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return RedirectToAction("Success");
    }
    [HttpPost]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Eyni səhifəyə qayıt və error göstər
            return View("QuickBooking", model);
        }

        // İstəyə uyğun Booking obyektini yarat və DB-ə əlavə et
        var booking = new Booking
        {
            CarId = model.CarId,
            CustomerName = model.CustomerName,
            Email = model.Email,
            Phone = model.Phone,
            PickupLocation = model.PickupLocation,
            DropoffLocation = model.DropoffLocation,
            PickupDate = model.PickupDate,
            ReturnDate = model.ReturnDate,
            Status = "Pending", // default status
            CreatedAt = DateTime.Now
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        // İstəyinə görə yönləndir
        return RedirectToAction("Dashboard", "Account");
    }

    public IActionResult Success()
    {
        return View();
    }
}

