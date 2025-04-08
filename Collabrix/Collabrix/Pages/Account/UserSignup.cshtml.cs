using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Collabrix.Models;
using Microsoft.AspNetCore.Authorization;
using Collabrix.Helper;
using Collabrix.Controllers;

namespace Collabrix.Pages.Account
{
    [AllowAnonymous]
    public class UserSignUpModel : PageModel
    {

        [BindProperty]
        public User? User { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        [BindProperty]
        public string? ConfirmPassword { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("User.PasswordHash");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }
            try
            {
                string hashedPassword = Password;
                User.PasswordHash = hashedPassword;
                await UserController.AddUser(User);
                return RedirectToPage("/Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}
