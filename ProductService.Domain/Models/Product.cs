namespace ProductService.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; } 
        public int Quantity { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
    }
}
