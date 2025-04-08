using Collabrix.Models;
using System.Data;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class TaskController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task ChangeTaskStage(int taskId, int newStageId)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "UPDATE Tasks SET ProjectTaskStageId = @NewStageId WHERE TaskId = @TaskId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewStageId", newStageId);
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        await command.ExecuteNonQueryAsync(); // Use ExecuteNonQueryAsync for better async support
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task<Tasks> GetTask(int taskId)
        {
            Tasks task = new Tasks();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                task.TaskId = reader.GetInt32(reader.GetOrdinal("TaskId"));
                                task.TaskName = reader.GetString(reader.GetOrdinal("TaskName"));
                                task.Description = reader.GetString(reader.GetOrdinal("Description"));
                                task.DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate"));
                                task.ProjectTaskStageId = reader.GetInt32(reader.GetOrdinal("ProjectTaskStageId"));
                                task.AssignedTo = reader.GetInt32(reader.GetOrdinal("AssignedTo"));
                                task.ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId"));
                                task.CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy"));
                                task.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                task.UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
                                task.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                            }
                            return await Task.FromResult(task);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task<List<Tasks>> GetTasks(int projectId)
        {
            List<Tasks> tasks = new List<Tasks>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Tasks WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                Tasks task = new Tasks
                                {
                                    TaskId = reader.GetInt32(reader.GetOrdinal("TaskId")),
                                    TaskName = reader.GetString(reader.GetOrdinal("TaskName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    DueDate = reader.GetDateTime(reader.GetOrdinal("DueDate")),
                                    ProjectTaskStageId = reader.GetInt32(reader.GetOrdinal("ProjectTaskStageId")),
                                    AssignedTo = reader.GetInt32(reader.GetOrdinal("AssignedTo")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                tasks.Add(task);
                            }
                            return await Task.FromResult(tasks);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task CreateTask(Tasks task)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    using (SqlCommand command = new SqlCommand("stpCreateTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Specify that it's a stored procedure
                        command.Parameters.AddWithValue("@TaskName", task.TaskName);
                        command.Parameters.AddWithValue("@Description", task.Description);
                        command.Parameters.AddWithValue("@DueDate", task.DueDate);
                        command.Parameters.AddWithValue("@ProjectTaskStageId", task.ProjectTaskStageId);
                        command.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                        command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
                        command.Parameters.AddWithValue("@CreatedBy", task.CreatedBy);
                        command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", task.UpdatedAt);
                        command.Parameters.AddWithValue("@IsDeleted", task.IsDeleted);

                        await command.ExecuteNonQueryAsync(); // Use ExecuteNonQueryAsync for better async support
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task EditTask(Tasks task)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    using (SqlCommand command = new SqlCommand("stpEditTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure; // Specify that it's a stored procedure
                        command.Parameters.AddWithValue("@TaskId", task.TaskId);
                        command.Parameters.AddWithValue("@TaskName", task.TaskName);
                        command.Parameters.AddWithValue("@Description", task.Description);
                        command.Parameters.AddWithValue("@DueDate", task.DueDate);
                        command.Parameters.AddWithValue("@ProjectTaskStageId", task.ProjectTaskStageId);
                        command.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                        command.Parameters.AddWithValue("@ProjectId", task.ProjectId);
                        command.Parameters.AddWithValue("@CreatedBy", task.CreatedBy);
                        command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt);
                        command.Parameters.AddWithValue("@UpdatedAt", task.UpdatedAt);
                        command.Parameters.AddWithValue("@IsDeleted", task.IsDeleted);

                        await command.ExecuteNonQueryAsync(); // Use ExecuteNonQueryAsync for better async support
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static async Task DeleteTask(int taskId)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); 
                    using (SqlCommand command = new SqlCommand("stpDeleteTask", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task UpdateTask(Tasks task, string action)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "";
                    if (action == "Update")
                    {
                        query = "UPDATE Tasks SET TaskName = @TaskName, Description = @Description, DueDate = @DueDate, ProjectTaskStageId = @ProjectTaskStageId, AssignedTo = @AssignedTo, UpdatedAt = @UpdatedAt WHERE TaskId = @TaskId";
                    }
                    else if (action == "Delete")
                    {
                        query = "UPDATE Tasks SET IsDeleted = 1 WHERE TaskId = @TaskId";
                    }
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskId", task.TaskId);
                        command.Parameters.AddWithValue("@TaskName", task.TaskName);
                        command.Parameters.AddWithValue("@Description", task.Description);
                        command.Parameters.AddWithValue("@DueDate", task.DueDate);
                        command.Parameters.AddWithValue("@ProjectTaskStageId", task.ProjectTaskStageId);
                        command.Parameters.AddWithValue("@AssignedTo", task.AssignedTo);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        await command.ExecuteNonQueryAsync(); // Use ExecuteNonQueryAsync for better async support
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async static Task<bool> IsStageInUse(int stageId)
        {
            Tasks task = new Tasks();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); 
                    string query = "SELECT count(taskId) FROM Tasks Where @stageId in (SELECT ProjectTaskStageId FROM Tasks)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@stageId", stageId);
                        int count = (Int32)command.ExecuteScalar();
                        if(count > 0)
                        {
                            return await Task.FromResult(true);
                        }
                        else
                        {
                            return await Task.FromResult(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
