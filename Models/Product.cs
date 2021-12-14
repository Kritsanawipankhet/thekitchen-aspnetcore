using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thekitchen_aspnetcore.Models
{
    public class Product
    {

        [Key]
        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Display(Name = "Image Name")]
        public string? Image { get; set; }

        [NotMapped]
        [Display(Name = "Upload File")]
        public IFormFile ImageFile { get; set; }
        public string? Description { get; set; }

        public int ? Price { get; set; }

        public int? Amount { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("CategoryId")]
        public Category? Categories { get; set; }
        public int? CategoryId { get; set; }
    }
}
