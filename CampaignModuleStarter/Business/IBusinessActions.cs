using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;

namespace SimpleCampaignModule.CampaignModuleStarter.Business
{
    public interface IBusinessActions
    {
        string OrderAction(Order order);
        string CreateCampaign(Campaign campaign);
        Campaign GetCampaignByName(string campaignName);
        string CreateProduct(Product product);
        Product GetProduct(string productCode);        
        string IncreaseTime(int hour);
    }
}