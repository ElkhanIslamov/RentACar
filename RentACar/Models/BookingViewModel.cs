using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace RentACar.Models
{
    public class BookingViewModel : IValidatableObject
    {
        public int? CarId { get; set; }
        public required string CarType { get; set; }
        public required string CustomerName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public required string Phone { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }

        public DateTime PickupDate { get; set; } = DateTime.UtcNow;
        public string PickupTime { get; set; } = "09:00";

        public DateTime ReturnDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public string ReturnTime { get; set; } = "09:00";

        public string? Message { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!TimeSpan.TryParse(PickupTime, out var pickupTimeSpan) ||
                !TimeSpan.TryParse(ReturnTime, out var returnTimeSpan))
            {
                yield return new ValidationResult("Invalid time format.");
                yield break;
            }

            var pickupDateTime = PickupDate.Date.Add(pickupTimeSpan);
            var returnDateTime = ReturnDate.Date.Add(returnTimeSpan);

            if (returnDateTime <= pickupDateTime)
            {
                yield return new ValidationResult("Return date and time must be after pickup date and time.",
                    new[] { nameof(ReturnDate), nameof(ReturnTime) });
            }

            if ((returnDateTime - pickupDateTime).TotalHours < 24)
            {
                yield return new ValidationResult("Booking duration must be at least 1 full day.",
                    new[] { nameof(PickupDate), nameof(ReturnDate) });
            }
        }
    }
}
