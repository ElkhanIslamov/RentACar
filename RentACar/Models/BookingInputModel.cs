namespace RentACar.Models
{
    public class BookingInputModel
    {
        public string VehicleType { get; set; }
        public string PickupLocation { get; set; }
        public string Destination { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupTime { get; set; }
        public DateTime ReturnDate { get; set; }
        public string ReturnTime { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Message { get; set; }
    }

}
