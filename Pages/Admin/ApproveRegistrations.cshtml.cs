using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Topcat_Cat_Hotel.Data;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class ApproveRegistrationsModel : PageModel
    {
        public AppDbContext _context { get; set; }
        public List<Registration> pendingRegistrations { get; set; }

        public ApproveRegistrationsModel(AppDbContext context)
        {
            _context = context;
        }

        public async void OnGet()
        {
            pendingRegistrations = await _context.Registrations
                .Include(r => r.Owner)
                .Where(r => r.Status == "pending")
                .ToListAsync();
        }
    }
}
