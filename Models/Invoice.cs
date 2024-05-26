using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Models
{
    public class Invoice
    {
        public int invoiceId { get; set; }
        public int bookingId { get; set; }
        public decimal amount { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public bool paid { get; set; }
        public DateTime createdAt { get; set; }
    }
}
