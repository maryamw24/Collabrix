
using Collabrix.Models;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Collabrix.Controllers
{
    public class LookUpcontroller
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async static Task<string> getValueById(int id)
        {
            string value = "";
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT Value FROM LookUp WHERE LookupId = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                value = reader.GetString(reader.GetOrdinal("Value"));
                            }
                            return await Task.FromResult(value);
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

        public static async Task<int> getIdByValue(string value)
        {
            int id = 0;
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = $"SELECT LookupId FROM LookUp WHERE Value = @value";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@value", value);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = reader.GetInt32(reader.GetOrdinal("LookupId"));
                            }
                            return await Task.FromResult(id);
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

        public async static Task<List<string>> getStatuses()
        {
            List<string> statuses = new List<string>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT Value FROM LookUp WHERE Category = 'Status'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                statuses.Add(reader.GetString(reader.GetOrdinal("Value")));
                            }
                            return await Task.FromResult(statuses);
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

        public async static Task<List<string>> getProjectTypes()
        {
            List<string> projectTypes = new List<string>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT Value FROM LookUp WHERE Category = 'ProjectType'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                projectTypes.Add(reader.GetString(reader.GetOrdinal("Value")));
                            }
                            return await Task.FromResult(projectTypes);
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

        public static async Task<List<Lookup>> GetLookupsByategory(string category)
        {
            List<Lookup> lookups = new List<Lookup>();
            string connectionString = Configuration.GetConnectionString("Default");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM LookUp WHERE Category = @category AND Value != 'Creater'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@category", category);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Lookup lookup = new Lookup
                                {
                                    LookupId = reader.GetInt32(reader.GetOrdinal("LookupId")),
                                    Category = reader.GetString(reader.GetOrdinal("Category")),
                                    Value = reader.GetString(reader.GetOrdinal("Value"))
                                };
                                lookups.Add(lookup);
                            }
                            return await Task.FromResult(lookups);
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
