using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Models
{
    public class Room
    {
        public int roomId { get; set; }
        [Required]
        public string roomNumber { get; set; }
        [Required]
        public string status { get; set; }
        public DateTime createdAt { get; set; }

        // Navigation property
        public ICollection<Booking> Bookings { get; set; }

        [NotMapped]
        public RoomStatus RoomStatus
        {
            get => Enum.Parse<RoomStatus>(status);
            set => status = value.ToString();
        }
    }
}
