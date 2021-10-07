using System.Collections.Generic;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;

namespace SimpleCampaignModule.CampaignModuleStarter.Persistence
{
    public class Database: IDatabase
    {
        public Database()
        {
            ProductCache  = new Dictionary<string, Product>();
            OrderCache = new Dictionary<string, List<Order>>();
            CampaignCache = new Dictionary<string, Dictionary<string, Campaign>>();
        }
    }
}