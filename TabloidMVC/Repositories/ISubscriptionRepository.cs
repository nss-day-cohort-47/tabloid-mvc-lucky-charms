using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ISubscriptionRepository
    {
        Subscription GetSubscriptionBySubPro(int subscriberId, int providerId);
        void AddSubscription(Subscription subscription);
        void RemoveSubscription(Subscription subscription);
    }
}
