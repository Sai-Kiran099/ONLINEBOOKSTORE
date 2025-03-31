using System.ComponentModel.DataAnnotations;

namespace ONLINEBOOKSTORE.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Author { get; set; }//string can be nullable

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}