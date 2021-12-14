using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace thekitchen_aspnetcore.Models
{
    public class OrderList
    {
        [Key]
        public int OrderListId { get; set; }


        [ForeignKey("OrderId")]
        public Order Orders { get; set; }
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("ProductId")]
        public Product Products { get; set; }
        public int ProductId { get; set; }
        [Required]
        public int OrderListProductAmount { get; set; }
        [Required]
        public int OrderListProductPrice { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
