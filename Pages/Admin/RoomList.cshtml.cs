using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;

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
            var rooms = await _context.Rooms
                .Include(r => r.Bookings)
                .ThenInclude(b => b.Cat)
                .ToListAsync();

            RoomList = rooms.Select(r => new RoomWithCatsDTO
            {
                RoomId = r.roomId,
                RoomNumber = r.roomNumber,
                RoomStatus = r.status,
                Cats = r.Bookings
                    .Where(b => b.status == "checkedIn" || b.status == "booked")
                    .Select(b => new RoomWithCatsDTO.CatDTO
                    {
                        CatId = b.catId,
                        Name = b.Cat.name,
                        CheckOutDate = b.checkOutDate,
                        CheckOutTime = TimeOnly.Parse(b.checkOutDate.ToString("HH:mm:ss")) // Adjust based on your time format
                    })
                    .ToList()
            }).ToList();
        }

        public async Task<IActionResult> OnPostSignOutCatsAsync(int roomId)
        {
            var room = await _context.Rooms
                .Include(r => r.Bookings)
                .ThenInclude(b => b.Cat)
                .FirstOrDefaultAsync(r => r.roomId == roomId);

            if (room != null)
            {
                foreach (var booking in room.Bookings.Where(b => b.status == "checkedIn"))
                {
                    booking.status = "checkedOut";
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
