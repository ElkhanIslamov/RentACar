using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.DataContext.Entities;
using RentACar.DataContext;
using Microsoft.AspNetCore.Identity;
using RentACar.Models;

namespace RentACar.Controllers;

[Authorize]
public class BookingController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public BookingController(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookingViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var booking = new Booking
        {
            CarType = model.CarType,
            PickupLocation = model.PickupLocation,
            DropoffLocation = model.DropoffLocation,
            PickupDate = model.PickupDate,
            PickupTime = model.PickupTime,
            ReturnDate = model.ReturnDate,
            ReturnTime = model.ReturnTime,
            CustomerName = user.FullName,
            Phone = user.PhoneNumber ?? "N/A",
            CarId = 1, // Test üçün, sonradan avtomobil seçimi əlavə edilə bilər
            CreatedAt = DateTime.Now
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();

        return RedirectToAction("AccountDashboard", "Account");
    }
}

