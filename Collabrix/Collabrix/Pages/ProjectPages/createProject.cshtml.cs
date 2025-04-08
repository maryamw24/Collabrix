using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;



namespace Collabrix.Pages
{
    public class createProjectModel : PageModel
    {
        [BindProperty]
        public Project Project { get; set; }
        [BindProperty]
        public string? MembersList { get; set; }
        [BindProperty]
        public string? StagesList { get; set; }
        [BindProperty]
        public List<Lookup>? Statuses { get; private set; }
        [BindProperty]
        public List<Lookup>? ProjectTypes { get; private set; }
        [BindProperty]
        public List<Lookup>? Roles { get; private set; }
        [BindProperty]
        public List<User>? Users { get; private set; }

        public string? stageName { get; set; }
        [BindProperty]
        public string? stageDescription { get; set; }
        [BindProperty]
        public Dictionary<string, int>? ProjectStages { get; set; }

        public async Task OnGet()
        {
            try
            {
                Statuses = await LookUpcontroller.GetLookupsByategory("ProjectStatus");
                ProjectTypes = await LookUpcontroller.GetLookupsByategory("ProjectType");
                Roles = await LookUpcontroller.GetLookupsByategory("Role");
                Users = await UserController.GetUsers();
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }

        }
        public async Task<IActionResult> OnPostCreateProject()
        {
            try
            {
                int userId = GetUId();
                if (userId == -1)
                {
                    TempData["ErrorOnServer"] = "User not found";
                    return RedirectToPage("/ProjectPages/createProject");
                }
                Project.CreatedBy = userId;
                Project.CreatedAt = DateTime.Now;

                int createdProjectId = await ProjectController.CreateProject(Project);
                if (MembersList != null)
                {

                    var selectedMembers = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(MembersList);
                    List<UserProject> userProjects = new List<UserProject>();
                    foreach (var member in selectedMembers)
                    {
                        UserProject userProject = new UserProject
                        {
                            UserId = Convert.ToInt32(member["id"]),
                            ProjectId = createdProjectId,
                            CreatedAt = DateTime.Now,
                            IsDeleted = false,
                            Role = Convert.ToInt32(member["role"])
                        };
                        await UserProjectController.CreateUserProject(userProject);
                    }

                }
                if (StagesList != null)
                {
                    var selectedStages = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(StagesList);
                    List<ProjectTaskStage> projectStages = new List<ProjectTaskStage>();
                    foreach (var stage in selectedStages)
                    {
                        ProjectTaskStage projectTaskStage = new ProjectTaskStage
                        {
                            ProjectId = createdProjectId,
                            StageName = stage["name"],
                            StageDescription = stage["description"],
                            CreatedBy = userId,
                            IsDeleted = false
                        };
                        await ProjectTaskStageController.CreateProjectTaskStage(projectTaskStage);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/allProjects");
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
