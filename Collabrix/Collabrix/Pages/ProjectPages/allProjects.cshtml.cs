using Collabrix.Controllers;
using Collabrix.Helper;
using Collabrix.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Collabrix.Pages
{
    public class allProjectsModel : PageModel
    {
        public List<Tuple<Project, string, string, bool>> Projects { get; set; }

        public async Task OnGet()
        {
            try
            {
                int userId = GetUId();
                if (userId == -1)
                {
                    TempData["ErrorOnServer"] = "User not found";
                    return;
                }
                Projects = new List<Tuple<Project, string, string, bool>>();
                List<Project> projects = await ProjectController.GetProjects(userId);
                foreach (Project project in projects)
                {
                    string creater = await UserProjectController.getCreater(project.ProjectId);
                    string initials = helper.GetInitials(creater);
                    bool isPermitted = await isPermittedForActions(project.ProjectId, userId);
                    this.Projects.Add(new Tuple<Project, string, string, bool>(project, creater, initials, isPermitted));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }
        private int GetUId()
        {
            var uIdClaim = User.FindFirst("uId");
            if (uIdClaim == null)
            {
                return -1;
            }

            return int.Parse(uIdClaim.Value);
        }

        private async Task<bool> isPermittedForActions(int projectId, int userId)
        {
            string Role = await LookUpcontroller.getValueById(await UserProjectController.getUserRole(projectId, userId));
            if(Role == "Creater" || Role == "Team Leader")
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }
    }
}
