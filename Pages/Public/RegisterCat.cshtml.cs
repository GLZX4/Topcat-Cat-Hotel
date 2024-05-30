using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
using Topcat_Cat_Hotel.Models;
using Topcat_Cat_Hotel.ViewModels;

namespace Topcat_Cat_Hotel.Pages.Public
{
    public class RegisterCatModel : PageModel
    {
        private readonly AppDbContext _context;
        public bool CodeValid { get; set; }
        public userSessionData User { get; set; }
        public RegisterCodeViewModel CodeInfo { get; set; }
        [BindProperty]
        public RegisterCatViewModel RegisterCatViewModel { get; set; } = new RegisterCatViewModel();


        public RegisterCatModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            LoadSessionData();
        }

        public async Task<IActionResult> OnPostCodeSubmissionAsync()
        {
            if (string.IsNullOrEmpty(CodeInfo.Code))
            {
                ModelState.AddModelError("CodeInfo.Code", "Code is required.");
                return Page();
            }

            var registrationCode = await _context.RegistrationCodes
                .FirstOrDefaultAsync(c => c.registrationCode == CodeInfo.Code);

            if (registrationCode == null)
            {
                ModelState.AddModelError("CodeInfo.Code", "Invalid code.");
                return Page();
            }

            if (registrationCode.isUsed)
            {
                ModelState.AddModelError("CodeInfo.Code", "This code has already been used.");
                return Page();
            }

            if (registrationCode.expiresAt < DateTime.Now)
            {
                ModelState.AddModelError("CodeInfo.Code", "This code has expired.");
                return Page();
            }

            // Store the valid code in the session
            HttpContext.Session.SetString("CodeValid", JsonSerializer.Serialize(true));
            HttpContext.Session.SetString("UserData", JsonSerializer.Serialize(User));
            HttpContext.Session.SetString("RegistrationCode", registrationCode.registrationCode);

            return RedirectToPage("/Public/RegisterCat");
        }

        public IActionResult OnPostSubmitNumOfCats()
        {
            LoadSessionData();

            // Add empty cat models to the list based on NumOfCats
            RegisterCatViewModel.Cats = new List<CatDetailsViewModel>();
            for (int i = 0; i < RegisterCatViewModel.NumOfCats; i++)
            {
                RegisterCatViewModel.Cats.Add(new CatDetailsViewModel { Index = i });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRegisterCatAsync()
        {
            Console.WriteLine("Registering cats...");
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Retrieve the stored code from the session
            if (!HttpContext.Session.TryGetValue("RegistrationCode", out var registrationCodeJson))
            {
                ModelState.AddModelError("", "Registration code not found.");
                return Page();
            }

            var registrationCode = JsonSerializer.Deserialize<string>(registrationCodeJson);

            var codeRecord = await _context.RegistrationCodes
                .FirstOrDefaultAsync(c => c.registrationCode == registrationCode);

            if (codeRecord == null)
            {
                ModelState.AddModelError("", "Registration code is invalid.");
                return Page();
            }

            // Mark the code as used
            codeRecord.isUsed = true;

            // Save the cats to the database
            RegisterCatViewModel.Cats.ForEach(cat =>
            {
                var catRecord = new Cat
                {
                    name = cat.Name,
                    breed = cat.Breed,
                    age = cat.Age,
                    sex = cat.Sex,
                    microchipped = cat.Microchipped,
                    microchipNumber = cat.MicrochipNumber,
                    insured = cat.Insured,
                    insuranceCompany = cat.InsuranceCompany,
                    insurancePolicyNumber = cat.InsurancePolicyNumber,
                };
                _context.Cats.Add(catRecord);
            });

            await _context.SaveChangesAsync();
            LoadSessionData();
            return RedirectToPage("/Success");
        }

        private void LoadSessionData()
        {
            if (HttpContext.Session.TryGetValue("UserData", out var userDataJson))
            {
                User = JsonSerializer.Deserialize<userSessionData>(userDataJson);
            }

            if (HttpContext.Session.TryGetValue("CodeValid", out var codeValidJson))
            {
                CodeValid = JsonSerializer.Deserialize<bool>(codeValidJson);
            }
        }
    }
}