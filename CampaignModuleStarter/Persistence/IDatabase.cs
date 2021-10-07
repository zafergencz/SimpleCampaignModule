using System.Collections.Generic;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;

namespace SimpleCampaignModule.CampaignModuleStarter.Persistence
{
    public abstract class IDatabase
    {
        public Dictionary<string, Product> ProductCache;
        public Dictionary<string, List<Order>> OrderCache;
        public Dictionary<string, Dictionary<string, Campaign>> CampaignCache;        
    }
}