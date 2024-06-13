using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Topcat_Cat_Hotel.Data;
using Topcat_Cat_Hotel.ViewModels;
using Topcat_Cat_Hotel.Services;
using Topcat_Cat_Hotel.DTO;
using System.Text.Json;
using Topcat_Cat_Hotel.Models.Enums;
using Topcat_Cat_Hotel.Models;
using System.Threading.Tasks;

using Passwordy_Authentication.Services;

namespace Topcat_Cat_Hotel.Pages.Admin
{
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        [BindProperty]
        public LoginViewModel LoginInfo { get; set; }

        [BindProperty]
        public RegisterViewModel RegisterInfo { get; set; }

        public AdminModel(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            ModelState.Clear();

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid!, You are in the login Method");
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == LoginInfo.Username);

            if (user != null && _passwordService.VerifyPassword(LoginInfo.Password, user.PasswordHash, user.PasswordSalt))
            {
                Console.WriteLine("User is logged in");

                var userSessionData = new userSessionData
                {
                    userId = user.UserId,
                    username = user.Username,
                    Role = user.Role,
                };

                var userDataJson = JsonSerializer.Serialize(userSessionData);
                HttpContext.Session.SetString("UserData", userDataJson);

                return RedirectToPage("/Admin/indexAdmin");
            }
            else
            {
                ModelState.AddModelError("Login", "Invalid username or password.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRegisterAsync()
        {
            Console.WriteLine("OnPostRegisterAsync Method started");

            var potentialUsers = _context.Users.Select(e => e.Username).ToList();

            if (potentialUsers.Contains(RegisterInfo.Username))
            {
                ModelState.AddModelError("Register", "Username already exists.");
                return Page();
            }

            if (RegisterInfo.Password != RegisterInfo.ConfirmPassword)
            {
                ModelState.AddModelError("Register", "Passwords do not match.");
                return Page();
            }

            var (HashedPassword, Salt) = _passwordService.HashPassword(RegisterInfo.Password);

            var newUser = new User
            {
                Username = RegisterInfo.Username,
                PasswordHash = HashedPassword,
                PasswordSalt = Salt,
                Role = RegisterInfo.Role.ToString(), // Convert enum to string
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            Console.WriteLine("User has been added to the database");

            return RedirectToPage("/Admin/Admin");
        }
    }
}
