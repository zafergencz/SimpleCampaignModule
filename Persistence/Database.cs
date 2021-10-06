using System.Collections.Generic;
using SimpleCampaignModule.Domain.Product;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Campaign;

namespace SimpleCampaignModule.Persistence
{
    public static class Database
    {
        public static Dictionary<string, Product> ProductCache  = new Dictionary<string, Product>();
        public static Dictionary<string, List<Order>> OrderCache = new Dictionary<string, List<Order>>();
        public static Dictionary<string, Dictionary<string, Campaign>> CampaignCache = new Dictionary<string, Dictionary<string, Campaign>>();

    }
}