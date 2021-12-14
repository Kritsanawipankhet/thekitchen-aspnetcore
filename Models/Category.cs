using System.ComponentModel.DataAnnotations;

namespace thekitchen_aspnetcore.Models
{
    public class Category
    {
        [Key]
        public int CategotyId { get; set; }

        [Required]
        public string? Name { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}
