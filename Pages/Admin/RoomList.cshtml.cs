using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class RoomListModel : PageModel
    {
        private readonly AppDbContext _context;

        public RoomListModel(AppDbContext context)
        {
            _context = context;
        }

        public List<RoomWithCatsDTO> RoomList { get; set; } = new List<RoomWithCatsDTO>();

        public async Task OnGetAsync()
        {
            try
            {
                // Fetch all rooms with their bookings and associated cats
                var rooms = await _context.Rooms
                    .Include(r => r.Bookings)
                    .ThenInclude(b => b.Cat)
                    .ToListAsync();

                // Map the rooms to RoomWithCatsDTO
                RoomList = rooms.Select(r => new RoomWithCatsDTO
                {
                    RoomId = r.roomId,
                    RoomNumber = r.roomNumber,
                    RoomStatus = r.status,
                    Cats = r.Bookings
                        .Where(b => (b.status == BookingStatus.checkedIn.ToString() || b.status == BookingStatus.checkedIn.ToString()) && b.Cat != null)
                        .Select(b => new RoomWithCatsDTO.CatDTO
                        {
                            CatId = b.catId,
                            Name = b.Cat.name,
                            CheckInDate = b.checkInDate,
                            CheckOutDate = b.checkOutDate,
                            CheckOutTime = new TimeOnly(12, 0) // Default check out time, adjust as needed
                        })
                        .ToList()
                }).ToList();

                // Ensure all rooms are included, even if they have no bookings
                foreach (var room in rooms)
                {
                    if (!RoomList.Any(r => r.RoomId == room.roomId))
                    {
                        RoomList.Add(new RoomWithCatsDTO
                        {
                            RoomId = room.roomId,
                            RoomNumber = room.roomNumber,
                            RoomStatus = room.status,
                            Cats = new List<RoomWithCatsDTO.CatDTO>()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<IActionResult> OnPostSignOutCatsAsync(int roomId)
        {
            Console.WriteLine("Entered SignOutCats method");
            Console.WriteLine($"Room ID: {roomId}");

            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .ThenInclude(b => b.Cat)
                .FirstOrDefaultAsync(r => r.roomId == roomId);

            if (room == null)
            {
                Console.WriteLine($"Room with ID {roomId} not found");
                return RedirectToPage();
            }

            Console.WriteLine($"Found room: {room.roomNumber}");
            Console.WriteLine($"Bookings in the room: {room.Bookings.Count}");

            bool anyCheckedIn = false;
            room.status = RoomStatus.available.ToString();
            foreach (var booking in room.Bookings)
            {
                Console.WriteLine($"Booking ID: {booking.bookingId}, Cat Name: {booking.Cat?.name}, Status: {booking.status}");
                if (booking.status == "checkedIn")
                {
                    Console.WriteLine($"Signing out cat: {booking.Cat.name} (Booking ID: {booking.bookingId})");
                    booking.status = "checkedOut";
                    anyCheckedIn = true;
                }
            }

            if (!anyCheckedIn)
            {
                Console.WriteLine("No cats were checked in for this room");
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Database save changes complete");

            return RedirectToPage();
        }

    }
}
