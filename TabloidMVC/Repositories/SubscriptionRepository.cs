using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class SubscriptionRepository : BaseRepository, ISubscriptionRepository
    {
        public SubscriptionRepository(IConfiguration config) : base(config) { }

        public Subscription GetSubscriptionBySubPro(int subscriberId, int providerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, SubscriberUserProfileId,
                            ProviderUserProfileId, BeginDateTime,
                            EndDateTime
                        FROM Subscription
                        WHERE SubscriberUserProfileId = @subscriberId
                            AND ProviderUserProfileId = @providerId";
                    cmd.Parameters.AddWithValue("@subscriberId", subscriberId);
                    cmd.Parameters.AddWithValue("@providerId", providerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Subscription subscription = null;

                    if (reader.Read())
                    {
                        subscription = new Subscription()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SubscriberUserProfileId = reader.GetInt32(reader.GetOrdinal("SubscriberUserProfileId")),
                            ProviderUserProfileId = reader.GetInt32(reader.GetOrdinal("ProviderUserProfileId")),
                            BeginDateTime = reader.GetDateTime(reader.GetOrdinal("BeginDateTime")),
                            EndDateTime = reader.GetDateTime(reader.GetOrdinal("EndDateTime"))
                        };
                    }
                    reader.Close();
                    return subscription;
                }
            }
        }

        public void AddSubscription(Subscription subscription)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription (
                            SubscriberUserProfileId,
                            ProviderUserProfileId,
                            BeginDateTime,
                            EndDateTime )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @subscriberUserProfileId,
                            @providerUserProfileId,
                            GETDATE(),
                            CAST('12/31/9999' as date) )";
                    cmd.Parameters.AddWithValue("@subscriberUserProfileId", subscription.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@providerUserProfileId", subscription.ProviderUserProfileId);

                    subscription.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void RemoveSubscription(Subscription subscription)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Subscription WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", subscription.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
