using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.DTO;
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

        public async Task<IActionResult> OnPostCodeSubmission()
        {
            var validCodes = _context.RegistrationCodes.Where(c => c.registrationCode == CodeInfo.Code).ToList();

            if (validCodes.Any())
            {
                HttpContext.Session.SetString("CodeValid", JsonSerializer.Serialize(true));
                return RedirectToPage("/Public/RegisterCat");
            }
            else
            {
                ModelState.AddModelError("CodeInfo.Code", "Invalid code");
                return Page();
            }
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

        public async Task<IActionResult> OnPostRegisterCat()
        {
            if (ModelState.IsValid)
            {
                // Process the registration
                // ...

                return RedirectToPage("/Success");
            }
            LoadSessionData();
            return Page();
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
