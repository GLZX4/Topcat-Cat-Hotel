using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.DTO
{
    public class invoiceDTO
    {
        public int invoiceId { get; set; }
        public int bookingId { get; set; }
        public decimal amount { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public bool paid { get; set; }
        public DateTime createdAt { get; set; }
        public DateOnly dueDate { get; set; }
        public bool overdue { get; set; }
        public string catName { get; set; }
        public string ownerName { get; set; }
    }
}
