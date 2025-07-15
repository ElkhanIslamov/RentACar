namespace RentACar.Models
{
    public class BookingViewModel
    {
        public int CarId { get; set; } // Gələcəkdə avtomobil seçimi əlavə oluna bilər
        public required string CarType { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }
        public DateTime PickupDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public DateTime ReturnDate { get; set; }
        public TimeSpan ReturnTime { get; set; }
    }
}
