using Microsoft.AspNetCore.Mvc.RazorPages;
using Collabrix.Models;
using Collabrix.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Collabrix.Pages.TaskPages
{
    public class ListModel : PageModel
    {
        [BindProperty]
        public int ProjectId { get; set; }
        [BindProperty]
        public Project? Project { get; private set; }
        [BindProperty]
        public List<Tasks>? Tasks { get; private set; }
        [BindProperty]
        public List<ProjectTaskStage>? Stages { get; private set; }
        [BindProperty]
        public List<UserProject>? Members { get; private set; }
        [BindProperty]
        public Tasks? Task { get; set; }
        [BindProperty]
        public int TaskId { get; set; }

        public async Task OnGet(int projectId)
        {
            try
            {
                ProjectId = projectId;
                Project = await ProjectController.GetProject(ProjectId);
                Tasks = await TaskController.GetTasks(ProjectId);
                Stages = await ProjectTaskStageController.GetProjectTaskStages(ProjectId);
                Members = await UserProjectController.GetMembers(ProjectId);
                ViewData["ProjectId"] = ProjectId;
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
        }
        public async Task<IActionResult> OnPostCreateEditTask()
        {
            try
            {
                if (Task.TaskId == 0)
                {
                    int userId = GetUId();
                    Task.CreatedAt = DateTime.Now;
                    Task.CreatedBy = userId;
                    Task.ProjectId = ProjectId;
                    Task.IsDeleted = false;
                    Task.UpdatedAt = DateTime.Now;
                    await TaskController.CreateTask(Task);
                    TempData["Success"] = "Task Created Successfully!!";
                }
                else
                {
                    Task.UpdatedAt = DateTime.Now;
                    Task.ProjectId = ProjectId;
                    await TaskController.EditTask(Task);
                    TempData["Success"] = "Task Updated Successfully!!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
            return RedirectToPage("/TaskPages/List", new { projectId = ProjectId });
        }

        public async Task<IActionResult> OnPostDeleteTask()
        {
            try
            {
                await TaskController.DeleteTask(TaskId);
                TempData["Success"] = "Task Deleted Successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorOnServer"] = ex.Message;
            }
            return RedirectToPage("/TaskPages/List", new { projectId = ProjectId });
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
