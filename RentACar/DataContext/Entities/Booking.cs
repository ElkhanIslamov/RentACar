﻿namespace RentACar.DataContext.Entities
{
    public class Booking
    {
        public int Id { get; set; }       
        public required string CarType { get; set; }
        public required string CustomerName { get; set; }
        public required string Phone { get; set; }
        public required string PickupLocation { get; set; }     
        public required string DropoffLocation { get; set; }
        public DateTime PickupDate { get; set; }    
        public TimeSpan PickupTime { get; set; }
        public DateTime ReturnDate { get; set; }  
        public TimeSpan ReturnTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int CarId { get; set; }
        public Car? Car { get; set; } 
    }

}
