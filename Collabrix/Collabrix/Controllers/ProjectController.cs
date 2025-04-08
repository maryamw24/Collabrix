using Collabrix.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Collabrix.Controllers
{
    public class ProjectController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<Project> GetProject(int projectId)
        {
            Project? project = null;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Projects WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                project = new Project
                                {
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                    ProjectType = reader.GetInt32(reader.GetOrdinal("ProjectTypeLookupId")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    Status = reader.GetInt32(reader.GetOrdinal("ProjectStatus")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                            return await Task.FromResult(project);
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

        public async static Task<List<Project>> GetProjects(int userId)
        {
            List<Project> projects = new List<Project>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * From Projects Where projectId in (Select projectId From UserProject Where userId = @UserId)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Use ReadAsync for better async support
                            {
                                Project project = new Project
                                {
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    ProjectName = reader.GetString(reader.GetOrdinal("ProjectName")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                    ProjectType = reader.GetInt32(reader.GetOrdinal("ProjectTypeLookupId")),
                                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                    Status = reader.GetInt32(reader.GetOrdinal("ProjectStatus")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))

                                };

                                projects.Add(project);
                            }
                            return await Task.FromResult(projects);
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

        public async static Task<int> CreateProject(Project project)
        {
            string connectionString = Configuration.GetConnectionString("Default");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("stpCreateProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                        command.Parameters.AddWithValue("@Description", project.Description);
                        command.Parameters.AddWithValue("@StartDate", project.StartDate);
                        command.Parameters.AddWithValue("@EndDate", project.EndDate);
                        command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
                        command.Parameters.AddWithValue("@CreatedBy", project.CreatedBy);
                        command.Parameters.AddWithValue("@ProjectStatus", project.Status);

                        SqlParameter projectIdParam = new SqlParameter("@ProjectId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(projectIdParam);

                        await command.ExecuteNonQueryAsync();

                        int projectId = (int)command.Parameters["@ProjectId"].Value;

                        return await Task.FromResult(projectId);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async static Task UpdateProject(Project project, string action)
        {
            string connectionString = Configuration.GetConnectionString("Default");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    if (action == "Update")
                    {
                        using (SqlCommand command = new SqlCommand("stpUpdateProject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Add input parameters
                            command.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                            command.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                            command.Parameters.AddWithValue("@Description", project.Description);
                            command.Parameters.AddWithValue("@StartDate", project.StartDate);
                            command.Parameters.AddWithValue("@EndDate", project.EndDate);
                            command.Parameters.AddWithValue("@ProjectType", project.ProjectType);
                            command.Parameters.AddWithValue("@ProjectStatus", project.Status);
                            command.Parameters.AddWithValue("@isDeleted", 0);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                    else if (action == "Delete")
                    {
                        using (SqlCommand command = new SqlCommand("stpDeleteProject", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Add input parameters
                            command.Parameters.AddWithValue("@isDeleted", 1);
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        public async static Task DeleteProject(int projectId)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Projects SET isDeleted = 1 Where projectId = @ProjectId";
                try
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        await command.ExecuteNonQueryAsync();
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

    }
}
