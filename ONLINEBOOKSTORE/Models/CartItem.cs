using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONLINEBOOKSTORE.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? BookId { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Quantity * Price;
        public string? ImageUrl { get; set; }

        public Book Book { get; set; }
    }

}