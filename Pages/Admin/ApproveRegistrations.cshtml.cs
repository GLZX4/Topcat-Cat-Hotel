using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Topcat_Cat_Hotel.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Topcat_Cat_Hotel.DTO;
using System.Text.Json;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class ApproveRegistrationsModel : PageModel
    {
        public AppDbContext _context { get; set; }
        public List<Registration> pendingRegistrations { get; set; }
        public List<Room> rooms { get; set; }
        public userSessionData sessionData { get; set; }


        public ApproveRegistrationsModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                sessionData = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            } else
            {
                Console.WriteLine("Session data not found");
            }

            pendingRegistrations = await _context.Registrations
                .Include(r => r.Owner)
                .Include(r => r.Cats)
                .Where(r => r.Status == "pending")
                .ToListAsync();

            rooms = await _context.Rooms.ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveRegistrationAsync(int registrationId, string action, int selectedRoom)
        {
            var registration = await _context.Registrations
                .Include(r => r.Cats)
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

            if (registration == null)
            {
                return NotFound();
            }

            if (action == "approve")
            {
                registration.Status = "approved";

                foreach (var cat in registration.Cats)
                {
                    var booking = new Booking
                    {
                        catId = cat.catId,
                        roomId = selectedRoom,
                        checkInDate = DateOnly.FromDateTime(registration.StartDate),
                        checkOutDate = DateOnly.FromDateTime(registration.EndDate),
                        status = BookingStatus.booked.ToString(),
                        createdAt = DateTime.Now
                    };
                    _context.Bookings.Add(booking);
                }

                // Update room status to occupied
                var room = await _context.Rooms.FindAsync(selectedRoom);
                if (room != null)
                {
                    room.status = RoomStatus.occupied.ToString();
                }
            }
            else if (action == "reject")
            {
                registration.Status = "declined";
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
