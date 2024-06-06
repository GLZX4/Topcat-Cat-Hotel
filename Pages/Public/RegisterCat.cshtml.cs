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

        [BindProperty]
        public RegisterCodeViewModel CodeInfo { get; set; }
        [BindProperty]
        public RegisterCatViewModel registerCat { get; set; } = new RegisterCatViewModel();

        public RegisterCatModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            LoadSessionData();

            if (registerCat.Cats == null || !registerCat.Cats.Any())
            {
                registerCat.Cats = new List<CatDetailsViewModel>();
                for (int i = 0; i < registerCat.NumOfCats; i++)
                {
                    registerCat.Cats.Add(new CatDetailsViewModel { Index = i });
                }
            }
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
                ModelState.AddModelError("CodeInfo.Code", "Invalid code");
                return Page();
            }

            if (registrationCode.isUsed)
            {
                ModelState.AddModelError("CodeInfo.Code", "This code has already been used");
                return Page();
            }

            if (registrationCode.expiresAt < DateTime.Now)
            {
                ModelState.AddModelError("CodeInfo.Code", "This code has expired");
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

            registerCat.Cats = new List<CatDetailsViewModel>();
            for (int i = 0; i < registerCat.NumOfCats; i++)
            {
                registerCat.Cats.Add(new CatDetailsViewModel { Index = i });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRegisterCatAsync()
        {
            LoadSessionData();
            Console.WriteLine("Registering cats...");
            ModelState.Clear();

            // Initialize Cats list if it's not already initialized
            if (registerCat.Cats == null || !registerCat.Cats.Any())
            {
                registerCat.Cats = new List<CatDetailsViewModel>();
                for (int i = 0; i < registerCat.NumOfCats; i++)
                {
                    registerCat.Cats.Add(new CatDetailsViewModel { Index = i });
                }
            }

            // Log the form data received
            Console.WriteLine("Received form data:");
            foreach (var key in Request.Form.Keys)
            {
                Console.WriteLine($"{key}: {Request.Form[key]}");
            }

            if (!TryUpdateModelAsync(registerCat).Result)
            {
                Console.WriteLine("Model binding failed. Errors:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            // Log the registerCat object to see if it contains the expected data
            string registerCatJson = JsonSerializer.Serialize(registerCat, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine("RegisterCat ViewModel after binding:");
            Console.WriteLine(registerCatJson);

            // Ensure Owner data is added first
            var owner = new Owner
            {
                name = registerCat.OwnersName,
                address = registerCat.OwnersAddress,
                postcode = registerCat.OwnersPostcode,
                mobile = registerCat.OwnersMobile,
                createdAt = DateTime.Now
            };

            _context.Owners.Add(owner);
            await _context.SaveChangesAsync(); // Save Owner to get the ownerId

            // Create a new Registration
            var registration = new Registration
            {
                OwnerId = owner.ownerId,
                ConsentToContactVet = true, //always true because they have to agree with the checkbox
                Status = "pending",
                CreatedAt = DateTime.Now,
                StartDate = registerCat.StartDate,
                EndDate = registerCat.EndDate
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync(); // Save Registration to get the registrationId
            bool michrochippedToConvert = false;
            bool insuredToConvert = false;

            

            // Add Cats
            foreach (var cat in registerCat.Cats)
            {
                //converting whatever the hell checkbox inputs output into actual booleans, i hate it.
                if (cat.Microchipped == "on")
                {
                    michrochippedToConvert = true;
                }

                if (cat.Insured == "on")
                {
                    insuredToConvert = true;
                }

                var catRecord = new Cat
                {
                    registrationId = registration.RegistrationId,
                    name = cat.Name,
                    breed = cat.Breed,
                    age = cat.Age,
                    sex = cat.Sex,
                    microchipped = michrochippedToConvert,
                    microchipNumber = cat.MicrochipNumber,
                    insured = insuredToConvert,
                    insuranceCompany = cat.InsuranceCompany,
                    insurancePolicyNumber = cat.InsurancePolicyNumber,
                    generalHealthDetails = cat.GeneralHealthDetails,
                    vetDetails = cat.VetDetails,
                    createdAt = DateTime.Now
                };
                _context.Cats.Add(catRecord);
            }

            await _context.SaveChangesAsync(); 

            LoadSessionData();
            if (User != null && User.Role.Equals("admin")) { 
                return RedirectToPage("/Admin/IndexAdmin");
            }
            else
            {
                return RedirectToPage("/Public/Index");
            }
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