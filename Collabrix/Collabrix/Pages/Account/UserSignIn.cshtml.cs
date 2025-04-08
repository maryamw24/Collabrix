using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;
using Collabrix.Models;
using Microsoft.AspNetCore.Authorization;
using Collabrix.Controllers;
using System.Data;
using System.Net;
using Collabrix.Helper;


namespace Collabrix.Pages.Account
{
    [AllowAnonymous]
    public class SignInModel : PageModel
    {
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public string? Password { get; set; }
        [BindProperty]
        private User User { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                bool isValid = await ValidateUser(Email, Password);
                if (isValid)
                {
                    var claims = new List<Claim>
                        {
                            new Claim("uId", User.UserId.ToString()),
                            new Claim(ClaimTypes.Name, User.FullName.ToString()),
                        };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToPage("/Index");
                }
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] += ex.Message;

                return Page();
            }
        }

        private async Task<bool> ValidateUser(string username, string password)
        {
            try
            {
                string passwordHash = password;
                User = await UserController.GetUser(username, passwordHash);
                if (User != null && !User.IsDeleted)
                {
                    //bool isValid = PasswordHasher.VerifyPassword(password, User.PasswordHash);
                    bool isValid = true;
                    if (isValid)
                    {
                        return true;
                    }
                    else
                    {
                        TempData["ErrorOnServer"] = "Invalid Credentials..!!";
                        return false;
                    }
                }
                else
                {
                    TempData["ErrorOnServer"] = "User not found";
                    return false;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
                return false;
            }
        }
    }

}