using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace W3.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string ISBN { get; set; }
        public DateTime DatePublished { get; set; } 
        public decimal Price { get; set; }
        public string Author { get; set; }
        public string? ImagePath { get; set; }
        
    }
}
