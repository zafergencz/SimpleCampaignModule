using System.Collections.Generic;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Persistence;

namespace SimpleCampaignModule.Tests.Persistence
{
    public class MockDatabase: IMockDatabase
    {
        public MockDatabase()
        {
            ClearCaches();
        }

        public override void ClearCaches()
        {
            ProductCache  = new Dictionary<string, Product>();
            OrderCache = new Dictionary<string, List<Order>>();
            CampaignCache = new Dictionary<string, Dictionary<string, Campaign>>();
        }
    }
}