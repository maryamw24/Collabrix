using Collabrix.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Collabrix.Controllers
{
    public class UserController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<User> GetUser(int userId)
        {
            User user = new User();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Users WHERE UserId = @UserId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")), // Change to int
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")), // Changed to "full_name"
                                    Email = reader.GetString(reader.GetOrdinal("Email")), // Changed to "email"
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")), // Changed to "password_hash"
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")), // Changed to non-nullable DateTime
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")), // Changed to non-nullable DateTime
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                            return await Task.FromResult(user);
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

        public async static Task<List<User>> GetUsers()
        {
            List<User> users = new List<User>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Users where isdeleted = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")), // Change to int
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")), // Changed to "full_name"
                                    Email = reader.GetString(reader.GetOrdinal("Email")), // Changed to "email"
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")), // Changed to "password_hash"
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")), // Changed to non-nullable DateTime
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")), // Changed to non-nullable DateTime
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                                users.Add(user);
                            }
                            return await Task.FromResult(users);
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

        public async static Task<User> GetUser(string email, string passwordHash)
        {
            User? user = null;
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "SELECT * FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync()) // Use ExecuteReaderAsync for better async support
                        {
                            while (await reader.ReadAsync()) // Use ReadAsync for better async support
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")), // Change to int
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")), // Changed to "full_name"
                                    Email = reader.GetString(reader.GetOrdinal("Email")), // Changed to "email"
                                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")), // Changed to "password_hash"
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")), // Changed to non-nullable DateTime
                                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")), // Changed to non-nullable DateTime
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                                };
                            }
                            return await Task.FromResult(user);
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
        public async static Task AddUser(User user)
        {
            string? connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync(); // Use OpenAsync for better async support
                    string query = "INSERT INTO Users (FullName, Email, PasswordHash, CreatedAt, UpdatedAt, IsDeleted) VALUES (@FullName, @Email, @PasswordHash, @CreatedAt, @UpdatedAt, @IsDeleted)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                        command.Parameters.AddWithValue("@IsDeleted", 0);
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
