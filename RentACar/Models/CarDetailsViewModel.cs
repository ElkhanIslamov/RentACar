namespace RentACar.Models
{
    public class CarDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public List<string> AdditionalImageUrls { get; set; } = new();
        public decimal PricePerDay { get; set; }
        public string? Description { get; set; }
    }
}
