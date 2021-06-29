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
    }
}
