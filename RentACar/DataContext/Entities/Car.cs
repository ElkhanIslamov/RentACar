namespace RentACar.DataContext.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public required string Name { get; set; }    
        public required string ImageUrl { get; set; }
        public int Seats { get; set; }  
        public int Doors { get; set; }  
        public int Luggage { get; set; }
        public decimal PricePerDay { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }

}
