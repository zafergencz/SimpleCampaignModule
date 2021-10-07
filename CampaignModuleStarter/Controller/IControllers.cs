using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;

namespace SimpleCampaignModule.CampaignModuleStarter.Controller
{
    public interface IControllers
    {
        string CreateProduct(Product product);
        string GetProductInfo(string productCode);
        string CreateOrder(Order order);
        string CreateCampaign(Campaign campaign);
        string GetCampaignInfo(string campaignName);
        string IncreaseTime(int hour);
    }
}