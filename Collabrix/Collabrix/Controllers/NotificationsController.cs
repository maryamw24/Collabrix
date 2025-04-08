using Collabrix.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace Collabrix.Controllers
{
    public class NotificationsController
    {
        private static IConfiguration Configuration { get; set; }
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static async Task<List<Notification>> GetNotifications()
        {
            List<Notification> notifications = new List<Notification>();
            string? connectionString = Configuration.GetConnectionString("Default");
            using(SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Notification";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) // Use ReadAsync for better async support
                            {
                                Notification notif = new Notification
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Message = reader.GetString(reader.GetOrdinal("Message")),
                                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                    IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                    NotificationTypeId = reader.GetInt32(reader.GetOrdinal("NotificationTypeId")),
                                    Read = reader.GetBoolean(reader.GetOrdinal("Read")),
                                    RecipientId = reader.GetInt32(reader.GetOrdinal("RecipientId")),
                                    SenderId = reader.GetInt32(reader.GetOrdinal("SenderId"))
                                };
                            }
                            return await Task.FromResult(notifications);
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
