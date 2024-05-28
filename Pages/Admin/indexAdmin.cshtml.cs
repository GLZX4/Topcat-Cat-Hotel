using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Models.Enums;
using Topcat_Cat_Hotel.Services;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class indexAdminModel : PageModel
    {
        private readonly WeatherService _weatherService;
        private readonly AppDbContext _context;
        private readonly PricingService _pricingService;

        public bool morning;
        public int occupied;
        public int catsDueIn;
        public List<Cat> catsDueInList { get; set; } = new List<Cat>();
        public int catsDueOut;
        public List<Cat> catsDueOutList { get; set; } = new List<Cat>();

        public decimal CurrentWeekIncome;
        public decimal NetIncome;

        public userSessionData sessionData { get; set; }

        public string weatherCondition { get; set; }

        public indexAdminModel(WeatherService weatherService, AppDbContext context, PricingService pricingService)
        {
            _weatherService = weatherService;
            _context = context;
            _pricingService = pricingService;
        }

        public async Task OnGetAsync()
        {
            weatherCondition = await _weatherService.GetWeatherConditionAsync();
            Console.WriteLine($"WeatherService returned: {weatherCondition}");

            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                sessionData = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            }
            else
            {
                Console.WriteLine("Session data not found");
            }

            morning = DateTime.Now.Hour < 12;

            await calculateOccupied();
            await calculateCatsDueIn();
            await calculateCatsDueOut();
            await calculateCurrentWeekIncome();
        }


        public async Task calculateOccupied()
        {
            occupied = await _context.Rooms.CountAsync(r => r.status == RoomStatus.occupied.ToString());
        }

        public async Task calculateCatsDueIn()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var bookingsDueIn = await _context.Bookings
                .Where(b => b.checkInDate == today && b.status == BookingStatus.booked.ToString())
                .ToListAsync();

            var catIdsDueIn = bookingsDueIn.Select(b => b.catId).ToList();
            catsDueInList = await _context.Cats.Where(c => catIdsDueIn.Contains(c.catId)).ToListAsync();

            catsDueIn = catsDueInList.Count;
        }

        public async Task calculateCatsDueOut()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var bookingsDueOut = await _context.Bookings
                .Where(b => b.checkOutDate == today && b.status == BookingStatus.checkedIn.ToString())
                .ToListAsync();

            var catIdsDueOut = bookingsDueOut.Select(b => b.catId).ToList();
            catsDueOutList = await _context.Cats.Where(c => catIdsDueOut.Contains(c.catId)).ToListAsync();

            catsDueOut = catsDueOutList.Count;
        }

        public async Task calculateCurrentWeekIncome()
        {
            var startOfWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek)));
            var endOfWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(7 - (int)DateTime.Now.DayOfWeek));
            var bookings = await _context.Bookings
                .Where(b => b.checkInDate >= startOfWeek && b.checkOutDate <= endOfWeek && (b.status == BookingStatus.booked.ToString() || b.status == BookingStatus.checkedIn.ToString()))
                .ToListAsync();

            CurrentWeekIncome = 0;

            foreach (var booking in bookings)
            {
                var numberOfCats = await _context.Cats.CountAsync(c => c.catId.Equals(booking.catId));
                var pricePerNight = _pricingService.GetPrice(numberOfCats);
                var duration = booking.checkOutDate.DayNumber - booking.checkInDate.DayNumber;
                CurrentWeekIncome += pricePerNight * duration;
            }
            NetIncome = CurrentWeekIncome * 0.8m; // 20% Tax
        }
    }
}
