using System.ComponentModel.DataAnnotations;
using thekitchen_aspnetcore.Data;

namespace thekitchen_aspnetcore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public virtual string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
        [Required]
        public string? OrderReceiverFirstname { get; set; }
        [Required]
        public string? OrderReceiverLastname { get; set; }
        [Required]
        public string? OrderReceiverPhone { get; set; }
        [Required]
        public string? OrderReceiverAddress { get; set; }
        [Required]
        public string? OrderReceiverZipcode { get; set; }
        [Required]
        public string? OrderReceiverProvince { get; set; }
        [Required]
        public string? OrderDeliveryName { get; set; }
        [Required]
        public int? OrderDeliveryPrice { get; set; }
        [Required]
        public int? OrderTotal { get; set; }
        public string? OrderPaymentReport { get; set; }

        public string? OrderDeliveryTrack { get ; set; }
        [Required]
        public int OrderStatus { get; set; }


        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    }
}
