﻿namespace W3.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public List<CartItem> Items { get; set; }
    }
}
