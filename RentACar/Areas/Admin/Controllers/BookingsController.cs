using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACar.DataContext;
using RentACar.DataContext.Entities;

namespace RentACar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BookingsController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings.Include(b => b.Car).ToListAsync();
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var booking = await _context.Bookings.Include(b => b.Car).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string status)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }

            // user məlumatını al (əgər login olarsa)
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                booking.CustomerName = user.FullName;
            }

            booking.CreatedAt = DateTime.Now;

            _context.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction("AccountDashboard", "Account");
        }

    }
}
