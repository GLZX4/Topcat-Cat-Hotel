using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class CalendarModel : PageModel
    {
        private readonly AppDbContext _context;

        public CalendarModel(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<DateTime, bool> Availability { get; set; }
        public userSessionData sessionData { get; set; }


        public async Task OnGetAsync()
        {
            Availability = await GetAvailabilityAsync(DateTime.Now, DateTime.Now.AddMonths(1));

            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                sessionData = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            }
            else
            {
                Console.WriteLine("Session data not found");
            }
        }

        private async Task<Dictionary<DateTime, bool>> GetAvailabilityAsync(DateTime startDate, DateTime endDate)
        {
            var bookings = await _context.Bookings
                .Where(b => b.checkInDate >= DateOnly.FromDateTime(startDate) && b.checkOutDate <= DateOnly.FromDateTime(endDate))
                .ToListAsync();

            var rooms = await _context.Rooms.ToListAsync();

            Dictionary<DateTime, bool> availability = new();

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                int bookedRooms = bookings.Count(b => b.checkInDate.ToDateTime(TimeOnly.MinValue) <= date && b.checkOutDate.ToDateTime(TimeOnly.MaxValue) >= date);
                availability[date] = bookedRooms < rooms.Count;
            }

            return availability;
        }
    }
}
