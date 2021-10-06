using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCampaignModule.Domain.Product;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Campaign;

namespace SimpleCampaignModule.Persistence
{
    public static class Database
    {
        public static Dictionary<string, Product> ProductCache  = new Dictionary<string, Product>();
        public static Dictionary<string, List<Order>> OrderCache = new Dictionary<string, List<Order>>();
        public static Dictionary<string, Campaign> ActiveCampaignCache = new Dictionary<string, Campaign>();
    }
}