using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Models
{
    public class Booking
    {
        public int bookingId { get; set; }
        public int catId { get; set; }
        public int roomId { get; set; }
        public string status { get; set; }
        public required DateOnly checkInDate { get; set; }
        public required DateOnly checkOutDate { get; set; }
        public DateTime createdAt { get; set; }

        // Navigation properties
        public Room Room { get; set; }
        public Cat Cat { get; set; }

        [NotMapped]
        public BookingStatus BookingStatus
        {
            get => Enum.Parse<BookingStatus>(status);
            set => status = value.ToString();
        }
    }
}
