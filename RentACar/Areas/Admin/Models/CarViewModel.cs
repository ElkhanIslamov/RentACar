using RentACar.DataContext.Entities;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Areas.Admin.Models
{
    public class CarViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }

        public int? Seats { get; set; }
        public int? Doors { get; set; }
        public int? Luggage { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? Categories { get; set; }
    }
}
