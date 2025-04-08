using Collabrix.Controllers;
using Collabrix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;
using System.Globalization;

namespace Collabrix.Pages.ProjectPages
{
    public class ProjectSettingModel : PageModel
    {
        private int _userId;
        [BindProperty]
        public Project? Project { get; set; }
        [BindProperty]
        public string? userRole { get; set; }
        [BindProperty]
        public int projectId { get; set;}
        public List<Lookup>? Statuses { get; private set; }
        public List<ProjectTaskStage>? ProjectTaskStages { get; private set; }
        public List<Lookup>? ProjectTypes { get; private set; }
        public List<Lookup>? Roles { get; private set; }
        public List<User>? Users { get; private set; }
        public List<UserProject>? ProjectUsers { get; private set; }

        [BindProperty]
        public UserProject? UserProject { get; set; }

        [BindProperty]
        public ProjectTaskStage? ProjectTaskStage { get; set; }

        public async Task OnGet(int projectId)
        {
            try
            {
                _userId = this.GetUId();
                userRole = await LookUpcontroller.getValueById(await UserProjectController.getUserRole(projectId, _userId));
                this.projectId = projectId;
                Project = await ProjectController.GetProject(this.projectId);
                ProjectUsers = await UserProjectController.GetProjectUsers(this.projectId);
                ProjectTaskStages = await ProjectTaskStageController.GetProjectTaskStages(this.projectId);
                Statuses = await LookUpcontroller.GetLookupsByategory("ProjectStatus");
                ProjectTypes = await LookUpcontroller.GetLookupsByategory("ProjectType");
                Roles = await LookUpcontroller.GetLookupsByategory("Role");
                Users = await UserController.GetUsers();
                ViewData["ProjectId"] = projectId;
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
        }

        public async Task<IActionResult> OnPostUpdateProject()
        { 
            try
            {
                await ProjectController.UpdateProject(Project, "Update");
                TempData["Success"] = "Project updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostUpdateRole()
            {
            try
            {
                await UserProjectController.UpdateUserRole(this.UserProject);
                TempData["Success"] = "Roles updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostAddMember()
        {
            try
            {
                await UserProjectController.CreateUserProject(this.UserProject);
                TempData["Success"] = "Member added successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostDeleteMember()
        {
            try
            {
                await UserProjectController.DeleteUserProject(this.UserProject);
                TempData["Success"] = "Member deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostUpdateStage()
        {
            try
            {
                await ProjectTaskStageController.UpdateTaskStage(ProjectTaskStage);
                TempData["Success"] = "Stage updated successfully!";
             }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostAddStage()
        {
            try
            {
                await ProjectTaskStageController.CreateProjectTaskStage(ProjectTaskStage);
                TempData["Success"] = "Stage added successfully!";
            }
            catch(Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }


        public async Task<IActionResult> OnPostDeleteStage()
        {
            try
            {
                bool flag = await TaskController.IsStageInUse(ProjectTaskStage.StageId);
                if (flag == false)
                {
                    await ProjectTaskStageController.DeleteTaskStage(ProjectTaskStage);
                    TempData["Success"] = "Stage deleted successfully!!";
                }
                else
                {
                    TempData["ErrorOnServer"] = "Stage is in use and cannot be deleted!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message + ex.StackTrace;
            }
            return RedirectToPage("/ProjectPages/ProjectSetting");
        }

        public async Task<IActionResult> OnPostDeleteProject()
        {
            try
            {
                await ProjectController.DeleteProject(projectId);
                TempData["Success"] = "Project deleted successfully!";
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
