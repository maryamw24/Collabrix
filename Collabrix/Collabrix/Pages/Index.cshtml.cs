using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Collabrix.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public List<Project>? Projects { get; set; }
        public async Task OnGet()
        {
            try
            {
                Projects = await ProjectController.GetProjects(GetUId());
                // set top 3 latest projects
                Projects = Projects.OrderByDescending(p => p.CreatedAt).Take(3).ToList();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }

        private int GetUId()
        {
            var uidClaim = User.FindFirst("uId");
            if (uidClaim == null)
            {
                return -1;
            }
            return int.Parse(uidClaim.Value);
        }
    }
}
