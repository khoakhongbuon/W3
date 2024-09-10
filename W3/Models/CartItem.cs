namespace W3.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string Name { get; set; }   
        
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ShoppingCart Cart { get; set; }
    }
}
