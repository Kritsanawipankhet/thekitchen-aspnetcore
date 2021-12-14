using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using thekitchen_aspnetcore.Data;

namespace thekitchen_aspnetcore.Models
{
    public class Cart 
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        public virtual string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public Product Products { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}
