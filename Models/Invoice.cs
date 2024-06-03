using System.ComponentModel.DataAnnotations.Schema;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Models
{
    public class Invoice
    {
        public int invoiceId { get; set; }
        public int bookingId { get; set; }
        public decimal amount { get; set; }
        public string paymentMethod { get; set; }
        public bool paid { get; set; }
        public DateTime createdAt { get; set; }

        // Navigation properties
        public Booking Booking { get; set; }

        [NotMapped]
        public PaymentMethod PaymentMethod
        {
            get => Enum.Parse<PaymentMethod>(paymentMethod);
            set => paymentMethod = value.ToString();
        }
    }
}