namespace RentACar.Models
{
    public class BookingViewModel
    {
        public int? CarId { get; set; }
        public string CarType { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupTime { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReturnTime { get; set; }
        public string? Message { get; set; }
    }

}
