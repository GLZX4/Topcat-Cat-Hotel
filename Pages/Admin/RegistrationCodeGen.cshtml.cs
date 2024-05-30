using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.Services;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class RegistrationCodeGenModel : PageModel
    {
        private readonly IKeyGeneratorService _keyGeneratorService;
        private readonly AppDbContext _context;

        public userSessionData sessionData { get; set; }
        public RegistrationCode generatedCode { get; set; }
        public string lastGeneratedCode { get; set; }

        public RegistrationCodeGenModel(AppDbContext context, IKeyGeneratorService keyGeneratorService)
        {
            _context = context;
            _keyGeneratorService = keyGeneratorService;
        }

        private void LoadSessionData()
        {
            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                sessionData = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            }
            else
            {
                Console.WriteLine("Session data not found");
            }
        }

        public async Task OnGetAsync()
        {
            LoadSessionData();
        }

        public async Task<IActionResult> OnPostGenerateCodeAsync()
        {
            LoadSessionData(); 
            Console.WriteLine("You are in the Generate Code Method");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid!, You are in the Generate Code Method");
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return Page();
            }

            generatedCode = new RegistrationCode
            {
                registrationCode = _keyGeneratorService.GenerateKey(),
                isUsed = false,
                createdAt = DateTime.Now,
                expiresAt = DateTime.Now.AddHours(6)
            };

            _context.RegistrationCodes.Add(generatedCode);
            await _context.SaveChangesAsync();

            lastGeneratedCode = generatedCode.registrationCode;

            return Page();
        }
    }
}
