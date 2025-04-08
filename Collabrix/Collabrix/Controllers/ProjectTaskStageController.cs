using Collabrix.Models;
using System.Data;
using System.Data.SqlClient;

namespace Collabrix.Controllers
{
    public class ProjectTaskStageController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<ProjectTaskStage>> GetProjectTaskStages(int projectId)
        {
            List<ProjectTaskStage> projectTaskStages = new List<ProjectTaskStage>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM ProjectTaskStage WHERE ProjectId = @ProjectId AND isDeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                ProjectTaskStage projectTaskStage = new ProjectTaskStage
                                {
                                    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    StageName = reader.GetString(reader.GetOrdinal("StageName")),
                                    StageDescription = reader.GetString(reader.GetOrdinal("StageDescription")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                projectTaskStages.Add(projectTaskStage);
                            }
                            return await Task.FromResult(projectTaskStages);
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

        public async static Task<ProjectTaskStage> GetProjectTaskStage(int stageId)
        {
            ProjectTaskStage projectTaskStage = new ProjectTaskStage();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM ProjectTaskStage WHERE StageId = @StageId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StageId", stageId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                projectTaskStage = new ProjectTaskStage
                                {
                                    StageId = reader.GetInt32(reader.GetOrdinal("StageId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    StageName = reader.GetString(reader.GetOrdinal("StageName")),
                                    StageDescription = reader.GetString(reader.GetOrdinal("StageDescription")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                            return await Task.FromResult(projectTaskStage);
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

        public async static Task CreateProjectTaskStage(ProjectTaskStage projectTaskStage)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("stpAddStageToProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProjectId", projectTaskStage.ProjectId);
                        command.Parameters.AddWithValue("@StageName", projectTaskStage.StageName);
                        command.Parameters.AddWithValue("@StageDescription", projectTaskStage.StageDescription);
                        command.Parameters.AddWithValue("@CreatedBy", projectTaskStage.CreatedBy);
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

        public async static Task UpdateTaskStage(ProjectTaskStage projectTaskStage)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Update ProjectTaskStage SET StageName = @StageName, StageDescription = @StageDescription WHERE projectId = @ProjectId AND stageId = @StageId";
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectTaskStage.ProjectId);
                        command.Parameters.AddWithValue("@StageId", projectTaskStage.StageId);
                        command.Parameters.AddWithValue("@StageDescription", projectTaskStage.StageDescription);
                        command.Parameters.AddWithValue("@StageName", projectTaskStage.StageName);
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

        public async static Task DeleteTaskStage(ProjectTaskStage projectTaskStage)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE ProjectTaskStage SET isDeleted = 1 WHERE ProjectId = @ProjectId AND StageId = @StageId";
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectTaskStage.ProjectId);
                        command.Parameters.AddWithValue("@StageId", projectTaskStage.StageId);
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
    }
}
