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
using Topcat_Cat_Hotel.Models.Enums;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class InvoicesModel : PageModel
    {
        public AppDbContext _context;

        public userSessionData sessionData { get; set; }
        public List<invoiceDTO>? invoices { get; set; }

        [BindProperty]
        public int InvoiceId { get; set; }

        [BindProperty]
        public PaymentMethod PaymentMethod { get; set; }

        public InvoicesModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                sessionData = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            }
            else
            {
                Console.WriteLine("Session data not found");
            }
            
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            invoices = await _context.Invoices
                .Include(i => i.Booking)
                    .ThenInclude(b => b.Cat)
                        .ThenInclude(c => c.Registration)
                            .ThenInclude(r => r.Owner)
                .Select(i => new invoiceDTO
                {
                    invoiceId = i.invoiceId,
                    bookingId = i.bookingId,
                    amount = i.amount,
                    paymentMethod = i.PaymentMethod,
                    paid = i.paid,
                    createdAt = i.createdAt,
                    dueDate = i.Booking.checkOutDate.AddDays(1),
                    overdue = currentDate > i.Booking.checkOutDate.AddDays(1),
                    catName = i.Booking.Cat.name,
                    ownerName = i.Booking.Cat.Registration.Owner.name
                })
                .ToListAsync();
        }

        public IActionResult OnPostSubmitInvoice()
        {
            // Handle form submission
            // Access InvoiceId and PaymentMethod properties here
            // Example: Save to database, etc.

            return RedirectToPage("Success");
        }
    }
}
