using Collabrix.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Collabrix.Controllers

{
    public class UserProjectController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<List<UserProject>> GetUserProjects(int userId)
        {
            List<UserProject> userProjects = new List<UserProject>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM UserProject WHERE UserId = @userId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                UserProject userProject = new UserProject
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                userProjects.Add(userProject);
                            }
                            return await Task.FromResult(userProjects);
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

        public async static Task<List<UserProject>> GetProjectUsers(int projectId)
        {
            List<UserProject> userProjects = new List<UserProject>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM UserProject WHERE ProjectId = @projectId AND isDeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@projectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                UserProject userProject = new UserProject();
                                userProject.UserId = reader.GetInt32(reader.GetOrdinal("UserId"));
                                userProject.ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId"));
                                userProject.CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
                                userProject.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
                                userProject.Role = reader.GetInt32(reader.GetOrdinal("Role"));
                                userProjects.Add(userProject);
                            }
                            return await Task.FromResult(userProjects);
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

        public async static Task<string> getCreater(int projectId)
        {
            string leader = "";
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT u.FullName From Projects P JOIN Users u On P.createdBy = u.UserId WHERE P.ProjectId = @projectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@projectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                leader = reader.GetString(reader.GetOrdinal("FullName"));
                            }
                            return await Task.FromResult(leader);
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

        public async static Task CreateUserProject(UserProject userProject)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("stpAddMemberToProject", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userProject.UserId);
                        command.Parameters.AddWithValue("@ProjectId", userProject.ProjectId);
                        command.Parameters.AddWithValue("@Role", userProject.Role);
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

        public async static Task<List<UserProject>> GetMembers(int projectId)
        {
            List<UserProject> userProjects = new List<UserProject>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM UserProject WHERE ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProjectId", projectId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                UserProject userProject = new UserProject
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    ProjectId = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    Role = reader.GetInt32(reader.GetOrdinal("Role"))
                                };
                                userProjects.Add(userProject);
                            }
                            return await Task.FromResult(userProjects);
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

        public async static Task UpdateUserRole(UserProject userProject)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "UPDATE UserProject SET Role = @Role WHERE UserId = @UserId AND ProjectId = @ProjectId AND isDeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Role", userProject.Role);
                        command.Parameters.AddWithValue("@UserId", userProject.UserId);
                        command.Parameters.AddWithValue("@ProjectId", userProject.ProjectId);
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

        public async static Task DeleteUserProject(UserProject userProject)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "UPDATE UserProject SET IsDeleted = 1 WHERE UserId = @UserId AND ProjectId = @ProjectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userProject.UserId);
                        command.Parameters.AddWithValue("@ProjectId", userProject.ProjectId);
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

        public async static Task<int> getUserRole(int projectId, int userId)
        {
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                int role = 0;
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT Role FROM UserProject WHERE UserId = @userId and ProjectId = @projectId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@projectId", projectId);
                        command.Parameters.AddWithValue("@userId", userId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                role = reader.GetInt32(reader.GetOrdinal("Role"));
                            }
                            return await Task.FromResult(role);
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
